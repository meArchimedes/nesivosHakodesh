import { Component, OnInit, Input, ViewChild, Output, EventEmitter, AfterViewInit } from '@angular/core';
import { NgbDateStruct, NgbInputDatepicker, NgbCalendar, NgbDatepickerI18n, NgbCalendarHebrew, NgbDatepickerI18nHebrew, NgbDate, NgbDatepicker } from '@ng-bootstrap/ng-bootstrap';
import { UtilService } from 'src/app/services/util.service';

@Component({
  selector: 'app-hebrew-datapicker',
  templateUrl: './hebrew-datapicker.component.html',
  styleUrls: ['./hebrew-datapicker.component.css'],
  providers: [
    {provide: NgbCalendar, useClass: NgbCalendarHebrew},
    {provide: NgbDatepickerI18n, useClass: NgbDatepickerI18nHebrew}
  ]
})
export class HebrewDatapickerComponent implements OnInit {


  @Input() date;
  @Input() toDate;
  @Input() dateRange: boolean = false;
  @Output() dateSelected = new EventEmitter<any>();

  model: NgbDateStruct;
  events: Array<any> = [];
  hoveredDate: NgbDate | null = null;

  constructor(private calendar: NgbCalendar, public i18n: NgbDatepickerI18n, private util:UtilService) {

    this.dayTemplateData = this.dayTemplateData.bind(this);

   }

  ngOnInit(): void {

    if(this.date) {
      
      var dd = this.util.getHebrewDate(this.date);
      this.date = new NgbDate(dd.getFullYear(), dd.getTishreiMonth(), dd.getDate());
      this.model = this.date;
    }

    if(this.toDate) {
      
      var dd = this.util.getHebrewDate(this.toDate);
      this.toDate = new NgbDate(dd.getFullYear(), dd.getTishreiMonth(), dd.getDate());
    }

  }
  

  dayTemplateData(date: NgbDate) {

    var eDate = (this.calendar as NgbCalendarHebrew).toGregorian(date);

    return {
      date: date,
      gregorian: eDate,
      hDate: this.util.getHebrewDate(new Date(eDate.year, eDate.month -1, eDate.day))
    };
  }

  monthChanged(e) {

    var firstDay = new NgbDate(e.next.year, e.next.month, 1);
    var gregorian = (this.calendar as NgbCalendarHebrew).toGregorian(firstDay);

    var events = this.util.getEventsForMonth(gregorian.year, gregorian.month - 1);
    this.events = events;
  }

  isHigh(d) {

    var showTypes = [5, 8, 10, 13, 33, 41, 1024, 16389];
    return this.events.filter(x => x.date.isSameDate(d.hDate) && showTypes.includes(x.mask)).length > 0;
  }

  GetEvent(d) {

    //if parshe
    var findP = this.events.filter(x => x.date.isSameDate(d.hDate) && x.mask == 1024 && x['parsha']);
    if(findP.length > 0) {
      return this.util.removeNekudot(findP[0].render('he').replace('־', ' ').replace('פרשת', '').trim());
    }

    //yom tov
    var typs = [5, 8, 10, 13, 33, 41, 16389];
    findP = this.events.filter(x => x.date.isSameDate(d.hDate) && typs.includes(x.mask));
    if(findP.length > 0) {
      return this.util.removeNekudot(findP[0].render('he'));
    }

    //other
    var typs = [0, 2, 64, 256, 16384];
    findP = this.events.filter(x => x.date.isSameDate(d.hDate) && typs.includes(x.mask));
    if(findP.length > 0) {
      return this.util.removeNekudot(findP[0].render('he'));
    }

    return '';

  }

  isHovered(date: NgbDate) {
    return this.date && !this.toDate && this.hoveredDate && date.after(this.date) && date.before(this.hoveredDate);
  }

  isInside(date: NgbDate) {
    return this.toDate && date.after(this.date) && date.before(this.toDate);
  }

  isRange(date: NgbDate) {
    return date.equals(this.date) || (this.toDate && date.equals(this.toDate)) /*|| this.isInside(date) || this.isHovered(date)*/;
  }

  dateSelect(e) {

    
    if(!this.dateRange) {

      this.date = e;
      this.dateSelected.emit(this.getDateObject(this.date));

    } else {

      if (!this.date && !this.toDate) {
        this.date = e;
      } else if (this.date && !this.toDate && e.after(this.date)) {
        this.toDate = e;

        //save
        var ret = {
          success: true,
          startDate: this.getDateObject(this.date),
          endDate: this.getDateObject(this.toDate)
        };
        this.dateSelected.emit(ret);

      } else {
        this.toDate = null;
        this.date = e;
      }
      
    }

  }

  clear() {

    this.dateSelected.emit({success: false});

  }

  getDateObject(e) {

    var gregorian = (this.calendar as NgbCalendarHebrew).toGregorian(e);
    var englishDate = new Date(gregorian.year, gregorian.month -1, gregorian.day);

    var dates = this.util.getHebrewDateNames(englishDate);
    dates.success = true;
    return dates;
  }
}
