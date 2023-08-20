import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { DeleteMaamarComponent } from '../delete-maamar/delete-maamar.component';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';
import { NewMaamarPopupComponent } from '../new-maamar-popup/new-maamar-popup.component';
import { Subject } from 'rxjs';
import { MergeParagraphPopupComponent } from '../merge-paragraph-popup/merge-paragraph-popup.component';
import { MergePopupComponent } from '../merge-popup/merge-popup.component';
import { ConditionalExpr } from '@angular/compiler';
import { element } from 'protractor';

@Component({
  selector: 'app-maamarim-list',
  templateUrl: './maamarim-list.component.html',
  styleUrls: ['./maamarim-list.component.css']
})
export class MaamarimListComponent implements OnInit {
  search: any = {
    Page: 1,
    ItemsPerPage: 20,
    Type :[],
    Topic: [],
    Parsha: [],
    Sefurim: [],
    Source: [],
    Status: [],
    dateRange: {},
    SearchTerm: '',
    sortBy: 'Date',
    sortDirection: 'Descending',
  }

  SourceSearch: any = {
    Page: 1,
    ItemsPerPage: 20,
    Type :[],
    Topic: [],
    Parsha: [],
    Source: [],
    sortBy: 'FirstName',
    sortDirection: 'Ascending',
  }

  TopiceSearch: any = {
    Page: 1,
    ItemsPerPage: 20,
    sortBy: 'Name',
    sortDirection: 'Ascending',
  }

  list: Array<any> = [];
  totalCount: number = 0;
  color: string
  prevSelectedid: number = 0;

  showFolderTabs: boolean = false;
  showFolders: boolean = false;
  showList: boolean = true;
  loadList: Array<any> = [];

  folderStructure: any = {};
  folderTabs: Array<any> = [];
  currFolderTab: any = {};

  ListOfType: Array<any> = [];
  ListOfSeforim: Array<any> = [];
  selectedTypes: Array<any> = [];
  ListOfStatus: Array<any> = [];
  ListOfSources: Array<any> = [];
  ListOfTopices: Array<any> = [];
  obListOfParsha2: Array<any> = [];
  ListOfYears: Array<any> = [];
  DateRangeString: string;


  @Input() isMergePopup: boolean = false;
  @Output() selectionUpdated = new EventEmitter<any>();

  public activeTab: number = 0;
  public maamarimListTabs = MaamarimListTabs;

  constructor(
    private modalService: BsModalService,
    private api: ApiService,
    private util: UtilService
  ) {}

  ngOnInit(): void {
    this.loadFolders();

    //check if we have a saved search object
    var savedSearch = this.util.GetAndDeleteSheardData('maamarim-list-saved-search');
    //console.log('savedSearch', savedSearch);

    if (savedSearch.value) {
      this.ListOfType = savedSearch.ListOfType;
      this.ListOfStatus = savedSearch.ListOfStatus;
      this.ListOfSources = savedSearch.ListOfSources;
      this.ListOfTopices = savedSearch.ListOfTopices;
      this.obListOfParsha2 =  savedSearch.obListOfParsha2;
      this.ListOfYears = savedSearch.ListOfYears;
      this.search = savedSearch.search;
      this.totalCount = savedSearch.totalCount;
      this.loadList = savedSearch.loadList;
      this.prevSelectedid = savedSearch.MaamarId;
      this.ListOfSeforim = savedSearch.ListOfSeforim;
      this.loadmaamarimList();
    } else {
      this.loadmaamarimList();
      this.loadTypes();
      this.loadSource()
      this.loadTopic()
      this.loadParshas();
      this.loadYears();
      this.loadStatus();
      this.loadListOfSeforim();
    }
  }

  groupSelectionAdd(e) {
    if (e.parsesObjects) {
      e.parsesObjects.forEach(x => {
        x.selected = true;
      });

      this.checked();
    } else {
      e.selected = true;
      this.checked();
    }
  }

  groupSelectionClear(elements) {
    elements.forEach(el => {
      el.parsesObjects.filter(x => x.selected).forEach(el => {
        delete el.selected;
      })
    });

    this.checked();
  }

  loadmaamarimList() {
    this.util.loadingStart();
    this.api.maamarimList(this.search, success => {
      this.loadList = success.list;
     // console.log('maamarimList', this.loadList);
      this.totalCount = success.totalCount;

      this.util.loadingStop();
    }, error => {
      this.loadList = [];
      console.log(error);
      this.util.loadingStop();
      this.util.openDeleteSource(error.messages)
    });
  }

  getSortClass(prop) {
    if(prop == this.search.sortBy) {
      return (this.search.sortDirection == 'Ascending' ? 'fas fa-sort-down' : 'fas fa-sort-up')
    }

    return 'fas fa-sort';
  }

  sort(prop) {
    if (prop == this.search.sortBy) {
      this.search.sortDirection = (this.search.sortDirection == 'Ascending' ? 'Descending' : 'Ascending')
    } else {
      this.search.sortBy = prop;
      this.search.sortDirection = 'Ascending';
    }

    this.search.Page = 1;
    this.prevSelectedid = 0;
    this.loadmaamarimList();
  }

  StatusColor(status){
    var colorDetails:any = {};

    switch (status) {
      case 'NewWithOutDetails':
       colorDetails.color = 'tag tag-red';
      break;
      case 'NeedDetails':
        colorDetails.color = 'tag tag-purple';
      break;
      case 'NotMuggedYet':
        colorDetails.color = 'tag tag-yellow';
      break;
      case 'NeedToWork':
        colorDetails.color = 'tag tag-light-Green';
      break;
      case 'Perfect':
        colorDetails.color = 'tag tag-green';
    }

    return colorDetails.color
  }

  pageChanged(e) {
    if (e != this.search.Page) {
      this.search.Page = e;
      this.prevSelectedid = 0;
      this.loadmaamarimList();
    }
  }

  itemsPerPageChanged(e) {
    this.search.Page = 1;
    this.search.ItemsPerPage = e;
    this.loadmaamarimList();
  }

  MaamarCheckedUpdates() {
    var selectedMaamarim = this.loadList.filter(x => x.selected);
    this.selectionUpdated.emit(selectedMaamarim);
  }

  loadFolders() {
    this.api.maamarimFolders(success => {
      this.folderTabs = success;
      this.currFolderTab = success[0];
    }, error => {
      console.log(error);
      this.util.openDeleteSource(error.messages)
    });
  }

  loadSource() {
    this.api.SourceList(this.SourceSearch, success => {
     this.ListOfSources = success.list;
    }, error => {
       console.log('error', error)
       this.util.openDeleteSource(error.messages)
    })
  }

  loadTopic(){
    this.api.GetTopicesForDropDown(success => {
      this.ListOfTopices = success;
    }, error => {
      console.log('error', error)
      this.util.openDeleteSource(error.messages)
    })
  }

  loadParshas(){
    this.api.getParshas2(success => {
      this.obListOfParsha2 = success;
    }, error => {
      console.log('error', error)
      this.util.openDeleteSource(error.messages)
    })
  }

  loadYears(){
    this.api.getAllYears(success => {
      this.ListOfYears = success
    }, error => {
      console.log('error', error)
      this.util.openDeleteSource(error.messages)
    })
  }

  loadTypes(){
    this.api.GetMaamarTypes(success => {
      this.ListOfType = success;
    }, error => {
      console.log('error', error)
      this.util.openDeleteSource(error.messages)
    })
  }

  loadListOfSeforim(){
    this.api.GetSeforimList(success => {
      this.ListOfSeforim = success;
   //  console.log('ListOfSeforim', this.ListOfSeforim)
    }, error => {
      this.util.loadingStop();
      this.util.openDeleteSource(error.messages)
    })
  }

  loadStatus(){
    this.api.GetMaamarStatus(success => {
      this.ListOfStatus = success;
    }, error => {
      console.log('error', error)
      this.util.openDeleteSource(error.messages)
    })
  }

  PopUpParagraph(maamarim){
    var modalRef = this.modalService.show(MergeParagraphPopupComponent, { class: "merge-modal-popup ngDraggable" , backdrop:'static', initialState: {
      contact: maamarim.maamarParagraphs,
    }});
  }

  onDropDownOpenChange(e) {
    if (!e) {
      this.checked();
    }
  }

  onClear(list) {
    list.filter(x => x.selected).forEach(el => {
      delete el.selected;
    });
  }

  SelectedType() {
    var text ="";

    for (var name in this.ListOfType.filter(x => x.selected)) {
      text += ", " + this.ListOfType.filter(x => x.selected)[name].typeValue;
    }

    return text;
  }

  SelectedTopic() {
    var text ="";

    for (var name in this.ListOfTopices.filter(x => x.selected)) {
      text += ", " + this.ListOfTopices.filter(x => x.selected)[name].name;
    }

    return text;
  }

  SelectedParsha() {
    var text ="";

    for (var name in this.GetAllSelectedParshas()) {
      text +=  ", " + this.GetAllSelectedParshas()[name].name;
    }

    return text;
  }

  SelectedYear() {
    var text ="";

    for (var name in this.ListOfYears.filter(x => x.selected)) {
      text += ", " + this.ListOfYears.filter(x => x.selected)[name].name;
    }

    return text;
  }

  SelectedSource() {
    var text ="";

    for (var name in this.ListOfSources.filter(x => x.selected)) {
      text +=  ", " + this.ListOfSources.filter(x => x.selected)[name].fullName;
    }

    return text;
  }

  SelectedStatus() {
    var text ="";

    for (var name in this.ListOfStatus.filter(x => x.selected)) {
      text +=  ", " + this.ListOfStatus.filter(x => x.selected)[name].typeValue;
    }

    return text;
  }

  SelectedCountType() {return this.ListOfType.filter(x => x.selected).length;}
  SelectedCountTopic() {return this.ListOfTopices.filter(x => x.selected).length;}
  SelectedCountSeforim() {return this.ListOfSeforim.filter(x => x.selected).length;}
  SelectedCountParsha() {return this.GetAllSelectedParshas().length;}
  SelectedCountSource() {return this.ListOfSources.filter(x => x.selected).length;}
  SelectedCountStatus() {return this.ListOfStatus.filter(x => x.selected).length;}
  SelectedCountYear() {return this.ListOfYears.filter(x => x.selected).length;}
  SelectedCountDate() {
    if (this.search.dateRange.endDate) {
      return 1
    }

    return 0;
  }

  checked() {
    this.search.Parsha = []
    this.search.Source = []
    this.search.Sefurim = [];
    this.search.Topic = []
    this.search.Type = []
    this.search.Years = [];
    this.search.Status = [];

    this.GetAllSelectedParshas().forEach(x => {
      this.search.Parsha.push(x.name);
    });

    this.ListOfYears.filter(x => x.selected).forEach(x => {
      this.search.Years.push(x.name);
    });

    this.ListOfTopices.filter(x => x.selected).forEach(x => {
      this.search.Topic.push(x.name);
    });
    this.ListOfSeforim.filter(x => x.selected).forEach(x => {
      this.search.Sefurim.push(x.name);
    });

    this.ListOfStatus.filter(x => x.selected).forEach(x => {
      this.search.Status.push(x.name);
    });

    this.ListOfType.filter(x => x.selected).forEach(x => {
      this.search.Type.push(x.name);
    });

    this.ListOfSources.filter(x => x.selected).forEach(x => {
      this.search.Source.push(x.sourceID);
    });

    this.search.Page = 1;
    this.loadmaamarimList();
  }

  changeTab(newTab) {
    if (this.activeTab != newTab && newTab == this.maamarimListTabs.firstTab) {
      this.showFolderTabs = false;
      this.showFolders = false;
      this.showList = true;
      this.activeTab = newTab;
    }

    if (this.activeTab != newTab && newTab == this.maamarimListTabs.secondTab) {
      this.showList = false;
      this.showFolderTabs = true;
      this.showFolders = true;
      this.activeTab = newTab;
    }
  }

  getFolderTabs() {
    return this.folderTabs.reverse();
  }

  openFolderAll() {
    if (this.currFolderTab.isHome) {
      this.showFolderTabs = false;
      this.activeTab = this.maamarimListTabs.firstTab;
    }

    this.populateSearchBasedOnFolders();
    this.showFolders = false;
    this.showList = true;
  }

  openFolder(fld) {
    this.folderTabs.push(fld);
    this.currFolderTab = fld;

    if (fld.folders && fld.folders.length > 0) {
      //it will go to the next folder
    } else {
      this.populateSearchBasedOnFolders();
      this.showFolders = false;
      this.showList = true;
    }
  }

  folderTabClick(fld, index) {
    if (index > 0) {
      for (let i = 0; i < index; i++) {
        this.folderTabs.pop();
      }

      this.currFolderTab = fld;
      this.showFolders = true;
      this.showList = false;
    }
  }

  populateSearchBasedOnFolders() {
    //first clear all current selections
    this.obListOfParsha2.forEach(element => {
      element.parsesObjects.forEach(x => {
        x.selected = false;
      });
    });

    this.ListOfType.forEach(element => {
      element.selected = false;
    });

    this.folderTabs.forEach(tab => {
      switch(tab.searchType) {
        case 'Types':
          var find = this.ListOfType.filter(x => x.name == tab.searchValue);

          if (find.length > 0) {
            find[0].selected = true;
          }

          break;
        case 'ParshaBook':
          var find = this.obListOfParsha2.filter(x => x.name == tab.searchValue);
          if (find.length > 0) {
            find[0].parsesObjects.forEach(x => {
              x.selected = true;
            });
          }

          break;
        case 'Parshas':
          //clear (if has selected also ParshaBook)
          this.obListOfParsha2.forEach(element => {
            element.parsesObjects.forEach(x => {
              x.selected = false;
            });
          });

          this.obListOfParsha2.forEach(element => {
            element.parsesObjects.forEach(x => {
              if (x.name == tab.searchValue) {
                x.selected = true;
              }
            });
          });

          break;
        case 'Year':
          var find = this.ListOfYears.filter(x => x.name == tab.searchValue);

          if (find.length > 0) {
            find[0].selected = true;
          }

          break;
        default:
      }
    });

    this.checked();
  }

  openMaamar(maamar) {
    if (this.isMergePopup) {
      this.PopUpParagraph(maamar);
    } else {
      //save search for next time
      var savedSearch = {
        value: true,
        search: this.search,
        ListOfType: this.ListOfType,
        ListOfSources: this.ListOfSources,
        ListOfTopices: this.ListOfTopices,
        obListOfParsha2: this.obListOfParsha2,
        ListOfYears: this.ListOfYears,
        totalCount: this.totalCount,
        loadList: this.loadList,
        ListOfStatus : this.ListOfStatus,
        MaamarId: maamar.maamarID,
        ListOfSeforim: this.ListOfSeforim,
      };

      this.util.SetSheardData('maamarim-list-saved-search', savedSearch);
      this.util.SetSheardData('maamar-' +  maamar.maamarID, maamar);
      this.util.navigate('maamarim/' + maamar.maamarID);
    }
  }

  DeleteMaamar(maamarId) {
    this.api.DeleteMaamar(maamarId, success => {
      this.loadmaamarimList();
    }, error => {
      console.log('error', error);
      //this.util.showError(error.messages)
      this.util.openDeleteSource(error.messages);
    }
  )}

  openMergeModal(Maamar) {
    var modalRef = this.modalService.show(MergePopupComponent, { class: "merge-modal ngDraggable", backdrop:'static', initialState: {
      Maamar: Maamar
    }});

    modalRef.content.onClose.subscribe(result => {
    //  console.log('result Merge Pop Up',result)
      //this.LoadMaamarDetails()
    })
  }

  openDeleteMaamar(maamarId) {
    var modalRef =  this.modalService.show(DeleteMaamarComponent, { class: "delete-maamar-modal ngDraggable", backdrop:'static', initialState: {
      contact: {
        Msg:  '?האם אתה בטוח שהנך רוצה למחוק את מאמר לצמיתות',
        Del: true
      }
    }})

    modalRef.content.onClose.subscribe(result => {
      if (result.success) {
        this.DeleteMaamar(maamarId)
      }
    })
  }

  openCreateModal() {
    var modalRef = this.modalService.show(NewMaamarPopupComponent, { class: "create-modal ngDraggable", backdrop: 'static',  initialState: {
      contact: {
        type: this.ListOfType,
        topic: this.ListOfTopices,
        source: this.ListOfSources,
      }
    }});

    modalRef.content.onClose.subscribe(result => {
      this.loadmaamarimList();
    })
  }

  filterDateSelected(e) {
    if (e.success) {
      this.DateRangeString = e.startDate.dmy + " - " + e.endDate.dmy;
      console.log('date', e);
      this.search.dateRange = {
        startDate: e.startDate.englishDate,
        endDate: e.endDate.englishDate
      };
    } else {
      this.search.dateRange = {};
    }

  //  console.log('date 2', this.search);
    this.loadmaamarimList();
  }

  GetAllSelectedParshas() {
    var selected = [];
    this.obListOfParsha2.forEach(pb => {
      pb.parsesObjects.forEach(x => {
        if (x.selected) {
          selected.push(x);
        }
      });
    });

    return selected;
  }

  isAllParshasChecked(s)  {
    return s.parsesObjects.every(x => x.selected);
  }

  checkParshaBook(s) {
    if (this.isAllParshasChecked(s)) {
      s.parsesObjects.forEach(x => {
        x.selected = false;
      });
    } else {
      s.parsesObjects.forEach(x => {
        x.selected = true;
      });
    }
  }


  //Get List of Main Safurim type
  ListOfMainSeferTypes(subTopics){
        return subTopics.filter(x => x.categoryName == "ספרים" && x.mainTopic == true)
  }
}

enum MaamarimListTabs {
  firstTab,
  secondTab
}



// this.loadList.forEach(Maamar => {
//   console.log('maamar', Maamar)
//   Maamar.subTopics.forEach(SubTopices => {
//    w =   SubTopices.filter(x => x.categoryName == "ספרים" && x.mainTopic == true)
//    });
// });
