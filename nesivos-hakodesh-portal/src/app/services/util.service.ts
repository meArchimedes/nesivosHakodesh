import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import {HDate, months, HebrewCalendar} from '@hebcal/core';
import swal from 'sweetalert2';
import { NgxSpinnerService } from 'ngx-spinner';
import gematriya from 'gematriya';
import { DeleteMaamarComponent } from '../components/main/maamarim/delete-maamar/delete-maamar.component';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { DomSanitizer } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
//const remote = require('electron').remote;

@Injectable({
  providedIn: 'root'
})
export class UtilService {

  private sharedData: any = {};
  private currentUser: any = {};
  private userList: Array<any> = [];

  constructor(
    private router: Router, 
    private location: Location, 
    private spinner: NgxSpinnerService, 
    private modalService: BsModalService,
    private toastr: ToastrService,
    private sanitizer: DomSanitizer,
    ) { }

  setCurrentUser(newUser) {
    this.currentUser = newUser;
  }
  getCurrentUser() {
    return this.currentUser;
  }

  setUserList(newList) {
    this.userList = newList;
  }
  getUserList() {
    return this.userList;
  }

  navigate(url) {
    this.router.navigateByUrl(url);
  }
  navigateNewWindow(url) {

    //console.log('navigateNewWindow', url);
    window.open(url, "_blank");

  }
  replaceState(url) {
    this.location.replaceState(url);
  }

  private showLoader: boolean = false;
  loadingStart(delay = 500) {
    this.showLoader = true;

    setTimeout(() => {

      if(this.showLoader) {
        this.spinner.show();
      }
    }, delay);
  }
  loadingStop() {
    this.showLoader = false;
    this.spinner.hide();
  }
  showToast() {
    this.toastr.success('!נשמר בהצלחה');
  }

  sanitizeUrl(url) {
    return this.sanitizer.bypassSecurityTrustUrl(url);
  }

  showSuccess(message, title = "הצלחה") {
    this.showSwal(message, title, "הצלחה");
  }
  
  showError(message, title = "שגיאה") {
    this.showSwal(message, title, "שגיאה");
  }

  showWarning(message, title = "אזהרה") {
    this.showSwal(message, title, "אזהרה");
  }

  showMessage(message, title) {
    this.showSwal(message, title, "מידע");
  }

  showSuccessToast(title = "נשמר בהצלחה") {

    swal.fire({
      title: title,
      //type: 'success',
      //position: 'bottom-start',
      showConfirmButton: false,
      timer: 1500
    });
  }

  showErrorToast(title = "E") {

    swal.fire({
      title: title,
      //type: 'warning',
      showConfirmButton: false,
      timer: 1500
    });
  }

  showConfirm(message, title = "אנא אשר", confirmText, success) {
    message = this.getSwalMessage(message);

    swal.fire({
      //type: "warning",
      title: title,
      html: message,
      showCancelButton: true,
      confirmButtonText: confirmText
    }).then((result) => {
      if(result.value) {
        success();
      }
    });
  }

  showConfirmWith(message, title = "אנא אשר", confirmText, success, cancel) {
    message = this.getSwalMessage(message);

    swal.fire({
      //type: "warning",
      title: title,
      html: message,
      showCancelButton: true,
      confirmButtonText: confirmText
    }).then((result) => {
      if(result.value) {
        success();
      } else {
        cancel();
      }
    });
  }

  showSwal(message, title, type) {
    message = this.getSwalMessage(message);

    swal.fire({
      //type: type,
      title: title,
      html: message
    });
  }

  getSwalMessage(message) {
    let text = '';
    if(Array.isArray(message)) {
      message.forEach(item => {
        text += item + "<br>";
      });
    } else if(typeof message === 'object' && Array.isArray(message.messages)) {
      // This usually means that the entire error response object was sent in
      message.messages.forEach(item => {
        text += item + "<br>";
      });
    } else if(typeof message === 'object' && message !== null) {
      // This is usually a .net object validation
      Object.keys(message).forEach(function(item) {
        text += message[item] + "<br>";
      });
    } else {
      text = message;
    }

    return text;
  }

  SetSheardData(key: string, value: any = {}) {
    this.sharedData[key] = value;
  }

  GetAndDeleteSheardData(key: string, defaultValue: any = {}) {

    let res = defaultValue;
    if (this.sharedData[key]) {
      res = this.sharedData[key];
      delete this.sharedData[key];
    }

    return res;
  }

  GetSheardData(key: string, defaultValue: any = {}) {

    let res = defaultValue;
    if (this.sharedData[key]) {
      res = this.sharedData[key];
    }

    return res;
  }

  getHebrewDate(date) {

    return new HDate(new Date(date));
  }

  private dateStringsCache = new Map();

  public getHebrewDateNames(englishDate) {

    if(!englishDate) {
      return {};
    }

    var date = new Date(englishDate);
    var key = `${date.getFullYear()}-${date.getMonth()}-${date.getDate()}`;

    if(!this.dateStringsCache.has(key)) {

      //add data to cache
      
      var d = new HDate(date);
      var hebewDayInWeek = this.getHebewDayName(d.getDay());
      var hebrewDate = gematriya(d.getDate());
      var hebrewMonthName = this.getHebewMonthName(d.getMonthName());
      var hebrewYearName = gematriya(d.getFullYear(), {limit: 3});
      var hebrewParsharName = this.getParsha(d);
      
      var data  = {
        englishDate: date,
        hDate: d,
        hebewDayInWeek: hebewDayInWeek,
        hebrewDate: hebrewDate,
        hebrewMonthName: hebrewMonthName,
        hebrewYearName: hebrewYearName,
        hebrewParsharName: hebrewParsharName,
        //ready formats
        dmy: `${hebrewDate} ${hebrewMonthName} ${hebrewYearName}`,
        dwy: `${hebewDayInWeek} ${hebrewParsharName} ${hebrewYearName}`,
        dwdmy: `${hebewDayInWeek} ${hebrewParsharName}, ${hebrewDate} ${hebrewMonthName} ${hebrewYearName}`,
      };

      this.dateStringsCache.set(key, data);

      //console.log('enter new data in cache ' + key, data);
    }

    return this.dateStringsCache.get(key);
  }

  GetDateForYearAndParsha(year, parsha) {

    var yearNumber = gematriya("ה" + year.replace("״", ''), {order: true});
    console.log('yearNumber', yearNumber);
    console.log('parsha', parsha);

    const options = {
      year: yearNumber,
      isHebrewYear: true,
      noHolidays: true,
      sedrot: true,
    };
    var parshas = HebrewCalendar.calendar(options);
    
    for (let i = 0; i < parshas.length; i++) {
      
      var ppp = this.removeNekudot(parshas[i].render('he').replace('־', ' ').replace('פרשת', '').trim());
      //console.log(ppp);
      
      if(ppp.includes(parsha)) {
        return parshas[i];
      }

    }

    return null;
  }

  private getParsha(d: HDate) {

    //got to shabas for this week
    var move = 6 - d.getDay();
    var shabbat = d;
    while (move > 0) {
      shabbat = shabbat.next();
      move--;
    }

    //get events for sabbat
    const options = {
      start: shabbat,
      end: shabbat,
      isHebrewYear: true,
      sedrot: true,
    };
    var findP = HebrewCalendar.calendar(options).filter(x => x['mask'] == 1024 && x['parsha']);
    var par = '';
    if(findP.length > 0) {
      par = this.removeNekudot(findP[0].render('he').replace('־', ' ').replace('פרשת', '').trim());
    }

    return par;
  }

  getEventsForMonth(year, month) {

    var currentMonth = new Date(year, month, 1);

    var prevMonth = new Date(year, currentMonth.getMonth() - 1, 1);

    var nextMonth = new Date(year, currentMonth.getMonth() + 2, 27);

    const options = {
      start: prevMonth,
      end: nextMonth,
      isHebrewYear: false,
      sedrot: true,
      //omer: true,
    };

    return HebrewCalendar.calendar(options);
  }

  private getHebewMonthName(month) {

    switch(month) {
      case 'Nissan':
        return 'ניסן';
      case 'Nisan':
        return 'ניסן';
      case 'Iyyar':
        return 'אייר';
      case 'Sivan':
        return 'סיון';
      case 'Tamuz':
        return 'תמוז';
      case 'Tammuz':
        return 'תמוז';
      case 'Av':
        return 'אב';
      case 'Elul':
        return 'אלול';
      case 'Tishrei':
        return 'תשרי';
      case 'Cheshvan':
        return 'חשון';
      case 'Kislev':
        return 'כסלו';
      case 'Tevet':
        return 'טבת';
      case "Sh'vat":
        return 'שבת';
      case 'Adar':
        return 'אדר';
      case 'Adar Rishon':
        return 'אדר';
      case 'Adar Sheini':
        return "'אדר ב";
      default:
        return month;
    }

  }

  private getHebewDayName(dayInWeek) {

    switch(dayInWeek) {
      case 0:
        return 'א';
      case 1:
        return 'ב';
      case 2:
        return 'ג';
      case 3:
        return 'ד';
      case 4:
        return 'ה';
      case 5:
        return 'ו';
      case 6:
        return 'שבת';
      default:
        return dayInWeek;
    }

  }

  removeNekudot(str) {
    return str.replace(/[\u0591-\u05C7]/g, '');
  }

  openDeleteSource(msg) {
    var modalRef =  this.modalService.show(DeleteMaamarComponent, { class: "delete-maamar-modal ngDraggable",  backdrop:'static',initialState: {
      contact:{
         title: 'שגיאה',
         Msg: msg,
         Del:  false  ,
         okButton: 'בסדר',
      }
      
    } })
  }

  EscapteHtml(string) {

    var tagsToReplace = {
        //'&': '&amp;',
        '<': '&msss;',
        '>': '&msse;',
        '/': '&mssfrw;'
    };
    return string.replace(/[&<>/]/g, function(tag) {
        return tagsToReplace[tag] || tag;
    });
  };

  CopyObject(obj) {
    return  JSON.parse(JSON.stringify(obj));
  }

  getAssestPrefix() {
    return environment.assestPrefix;
  }

  getIndexOptions() {
    return [
      'א',
      'ב',
      'ג',
      'ד',
      'ה',
      'ו',
      'ז',
      'ח',
      'ט',
      'י',
      'יא',
      'יב', 
      'יג',
      'יד',
      'טו',
      'טז', 
      'יז',
      'יח',
      'יט',
      'כ'
    ];
  }
}
