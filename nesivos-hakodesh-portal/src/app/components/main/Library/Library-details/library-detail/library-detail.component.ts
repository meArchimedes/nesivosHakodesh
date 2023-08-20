import { Component, OnInit, AfterViewChecked, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';
import { ActivatedRoute } from '@angular/router';
import gematriya from 'gematriya';

@Component({
  selector: 'app-library-detail',
  templateUrl: './library-detail.component.html',
  styleUrls: ['./library-detail.component.css']
})
export class LibraryDetailComponent implements OnInit, AfterViewChecked {
  backToSearchResult: boolean = false;
  public tabs = Tabs;
  public activeTab: number = 0;
 fortSize: number = 23;
  Libraryid
  MaamarimLinks: Array<any> = [
    {title: ''}
  ];
  LibraryDeitails: any = {}

  searchCriteria: any = {
    Page: 1,
    itemsPerPage: 30,
    ChepterId: 1,
    libraryCategory:  [],
    libraryType: [],
    LibrarySection:  [],
    LibraryChepter:  [],
  };

  LibrarySeferdetails : any = [{
    section: ''
  }]

 

  private _scrolled: boolean = false;

  constructor(
    @Inject(DOCUMENT)

    private _document: Document,
    private api: ApiService,
    public util: UtilService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.Libraryid = this.route.snapshot.paramMap.get('id');
    this.Libraryid = parseInt(this.Libraryid, 10)
    this.loadLibraryDetails(this.Libraryid)
  }

  ngAfterViewChecked(): void {
    if (this._scrolled === false) {
      this.scrollToHighlight();
    }
  }

  scrollToHighlight() {
    const highlightItem = this._document.querySelector('.table-row.selected');

    if (highlightItem) {
      highlightItem.scrollIntoView({
        behavior: 'smooth',
        block: "center"
      });

      this._scrolled = true;
    }
  }

  loadLibraryDetails(id){
    this.searchCriteria = {
      libraryCategory: [],
      libraryType:[],
      LibrarySection:[],
      LibraryChepter: '',
      LibraryId : id,
      ChepterId: 1,
      // Page: page,
      itemsPerPage: 30,
    }

    this.api.LibraryDetails(this.searchCriteria, success => {
     
     // console.log(' this.Libraryid',  this.Libraryid)
      this.LibrarySeferdetails = success;
       console.log('seccess library Details', this.LibrarySeferdetails)
      var yearNumber = gematriya("לג" , {order: true});
     // console.log('yearNumber', yearNumber)
      this.getChepterAlphaBetToNumber()
      // this.LibrarySeferdetails = this.LibrarySeferdetails.concat(success)
    }, error => {
      console.log(error)
    })

    this.clicked(id)
  }

  //get the Chepter alphabet Convert to number
  getChepterAlphaBetToNumber(){
  var firstVerseFromChepter =   this.LibrarySeferdetails.filter(x => x.libraryId == this.Libraryid)
 // console.log('aaaaaa', firstVerseFromChepter[0])
     var ChepterNumber = gematriya(firstVerseFromChepter[0].chepter , {order: true});
     //console.log('alphabet Number', ChepterNumber)
      this.searchCriteria.ChepterId = ChepterNumber
  }

  Scroll(site) {
  //  console.log();

    var lastItem = Math.max(...this.LibrarySeferdetails.map(x => x.sortBy));
    var firstItem = Math.min(...this.LibrarySeferdetails.map(x => x.sortBy));
  //  console.log('up firstItem', firstItem)
  //  console.log('lastItem', lastItem)

    if (site == 'down') {
     // this.searchCriteria.StartSort = lastItem + 1;
     // this.searchCriteria.EndSort =  lastItem + 30;

   //  console.log( ' this.searchCriteria.ChepterId before', this.searchCriteria.ChepterId)
      this.searchCriteria.ChepterId =  this.searchCriteria.ChepterId + 1
     // console.log( ' this.searchCriteria.ChepterId', this.searchCriteria.ChepterId)
    }
    else  {
    //  this.searchCriteria.StartSort = firstItem - 30;
     // this.searchCriteria.EndSort = firstItem - 1;
   //  console.log( ' this.searchCriteria.ChepterId before', this.searchCriteria.ChepterId)
     if (this.searchCriteria.ChepterId != 1) {
      this.searchCriteria.ChepterId =  this.searchCriteria.ChepterId - 1
     }
      
     // console.log( ' this.searchCriteria.ChepterId :next', this.searchCriteria.ChepterId)
    }

    this.searchCriteria.LibraryId = this.Libraryid;
    //console.log('scroll 2', this.searchCriteria)

    this.api.LibraryDetailsForScroll(this.searchCriteria, success => {
      // this.LibrarySeferdetails. success.list;

    //  console.log('success befor sort',  this.LibrarySeferdetails)
     // console.log('success',  success.list)
      if (success.list.length > 0) {
             // this.LibrarySeferdetails = this.LibrarySeferdetails.concat(success.list)
             this.LibrarySeferdetails = success.list
             this.LibrarySeferdetails = this.LibrarySeferdetails.sort((a, b) => (a.sortBy > b.sortBy) ? 1 : -1)
            // console.log('seccess library Details',  this.LibrarySeferdetails)
      }

    }, error => {
      console.log(error)
    })
  }

  Back() {
    this.util.navigate('library');
  }

  openMaamarLink(MaamarLink) {
    var id = MaamarLink.maamar?.maamarID;
    this.util.navigateNewWindow(`/maamarim/${id}`);
  }

  deleteMaamarLink(MaamarLink, e) {
    if (e != null) {
      e.stopPropagation();
    }

    this.api.DeleteMaamarLibraryLink(MaamarLink.maamarLibraryLinkId, success => {
      this.clicked(this.Libraryid)
    }, error => {})

    //console.log('MaamarLink', MaamarLink)
  }

  clicked(libraryId) {
    this.Libraryid = libraryId
    this.LibraryDeitails =
    this.api.maamarLibraryLinks(libraryId , success => {
     // console.log('maamarLibraryLinks', success)
      this.MaamarimLinks = success
      console.log('maamarLibraryLinks', this.MaamarimLinks)
    }, error => {
      console.log(error)
    })
  }


  //zoom in or out

  Zoom(Way){
  // console.log('Zoom way', Way)
   if (Way == 'plus') {
    this.fortSize++
   } else {
    this.fortSize =  this.fortSize - 1
   }
  }
}

enum Tabs {
  sources,
  links,
  generalInfo
}


