import { Component, OnInit } from '@angular/core';
import { FileDetector } from 'protractor';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';

@Component({
  selector: 'app-library-list',
  templateUrl: './library-list.component.html',
  styleUrls: ['./library-list.component.css']
})
export class LibraryListComponent implements OnInit {
  ListOfCategorys: any = []
  List: any = []
  folderTabs: Array<any> = [];
  searchCriteria: any = {
    Page: 1,
    itemsPerPage: 10,
    libraryCategory:  [],
    libraryType: [],
    LibrarySection:  [],
    LibraryChepter:  [],
  };
  constructor(private api: ApiService,  private util: UtilService) { }

  ngOnInit(): void {
    this.loadCategoryFilters();
    var l = {
      text:'ראשי',
      type : 'home',
      isHome: true
    }
 this.folderTabs.push(l);

  }

  getFolderTabs() {
    return this.folderTabs.reverse();
  }

  loadCategoryFilters(){
    this.List = []
    this.api.GetCategoryFilters(success => {
     // console.log('success', success)
      this.ListOfCategorys = success;
      this.ListOfCategorys.forEach(e => {

        var l = {
          text: e,
          type : 'Category',
        }
     this.List.push(l);
     
});
 //console.log('this.ListOfCategorys', this.List)
    }, error => {
       console.log(error)
      //this.util.showError(error.messages);
     })
  }

  loadTypeFilters(){
    this.List = []
    this.api.GetTypeFilters(this.searchCriteria.libraryCategory, success => {
     //console.log('Success Types', success);
      this.ListOfCategorys = success;
      this.ListOfCategorys.forEach(e => {

        var l = {
          text: e,
          type : 'type',
        }
     this.List.push(l);
     
});
}, error => {
      console.log(error)
    })
  }

  loadSectionFilters(){
    this.List = []
   // console.log('close Sec', this.searchCriteria.libraryType)

    this.api.GetSectionFilters(this.searchCriteria, success => {
     // console.log('Success Types', success);
      this.ListOfCategorys = success;
      this.ListOfCategorys.forEach(e => {

        var l = {
          text: e,
          type : 'section',
        }
     this.List.push(l);
     
});
    }, error => {
      console.log(error)
    })
  }

  loadChepterFilters(){
    this.List = []
   // console.log('close Sec2', this.searchCriteria)
    this.api.GetChepterFilters(this.searchCriteria, success => {
    //  console.log('Success Types', success);
      this.ListOfCategorys = success;
      this.ListOfCategorys.forEach(e => {

        var l = {
          text: e,
          type : 'chepter',
        }
     this.List.push(l);
     
});
    }, error => {
      console.log(error)
    })
  }

  folderTabClick(fld, index) {
  //  console.log('index', index, fld)

    switch (fld.type) {
      case 'home':
        this.loadCategoryFilters()
        break;
      case 'Category':
        this.loadTypeFilters()
        break;
      case 'type':
        this.loadSectionFilters();
        break;
      case 'section':
        this.loadChepterFilters()
        break;
      case 'chepter':
       
        break;
      
    }

//console.log('search object', this.searchCriteria)
    if(index > 0) {

      for (let i = 0; i < index; i++) {
        this.folderTabs.pop();
      }

     
      
    }
  }

  openFolder(fld){
    //console.log('fld', fld)
    this.folderTabs.push(fld);
   // console.log('search cri', this.searchCriteria)



    switch (fld.type) {
      case 'Category':
        this.searchCriteria.libraryCategory = []
        this.searchCriteria.libraryCategory.push(fld.text)
        this.loadTypeFilters()
        break;
      case 'type':
        this.searchCriteria.libraryType = []
        this.searchCriteria.libraryType.push(fld.text)
        this.loadSectionFilters();
        break;
      case 'section':
        this.searchCriteria.LibrarySection = []
        this.searchCriteria.LibrarySection.push(fld.text)
        this.loadChepterFilters()
        break;
      case 'chepter':
        this.searchCriteria.LibraryChepter = []
        this.searchCriteria.LibraryChepter.push(fld.text)
        this.searchCriteria.itemsPerPage = 1
      this.api.LibraryList(this.searchCriteria, success => {
      //  console.log('seccess library Details id', success.list)

        this.util.navigate('library/' + success.list[0].libraryId);
       // this.LibrarySeferdetails. success.list;
       
      }, error => {
  console.log(error)
      })

        break;
      
    }


  //   if (this.searchCriteria.libraryCategory.length == 0) {
  //     this.searchCriteria.libraryCategory.push(fld.text)
  //     this.loadTypeFilters();
  //     console.log('search cri 2', this.searchCriteria)
  //   } else if (this.searchCriteria.libraryType.length == 0){
  //     this.searchCriteria.libraryType.push(fld.text)
  //     this.loadSectionFilters();
  //     console.log('search cri 3', this.searchCriteria)
  //   }

  //   else if (this.searchCriteria.LibrarySection.length == 0){
  //     this.searchCriteria.LibrarySection.push(fld.text)
  //     this.loadChepterFilters();
  //     console.log('search cri 4', this.searchCriteria)
  //   }
  //   else  {
  //     this.searchCriteria.LibraryChepter.push(fld.text)
  //       this.searchCriteria.itemsPerPage = 1
  //     this.api.LibraryList(this.searchCriteria, success => {
  //       console.log('seccess library Details id', success.list)

  //       this.util.navigate('library/' + success.list[0].libraryId);
  //      // this.LibrarySeferdetails. success.list;
       
  //     }, error => {
  // console.log(error)
  //     })
  //   }

   }
}
