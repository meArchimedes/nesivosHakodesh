import { Component, OnInit } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';
import { DeleteMaamarComponent } from '../../maamarim/delete-maamar/delete-maamar.component';
import { SourcesDetailsPopupComponent } from '../../sources/sources-details-popup/sources-details-popup.component';
import { AddUserComponent } from '../add-user/add-user.component';
import { CategoryPopupComponent } from '../Category-PopUp/Category-popup.component';
import { UserPopupComponent } from '../user-popup/user-popup.component';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  public activeTab: number = 0;
  public toggleTabs = ToggleTabs;

  CategorySearch: any = {
    Page: 1,
    ItemsPerPage: 20,
    sortBy: 'CategoryName',
    sortDirection: 'Ascending',

   }

  search: any = {
    Page: 1,
    ItemsPerPage: 20,
    sortBy: 'Name',
    sortDirection: 'Ascending',
    CategoryId: 0
    
  }

  UserSearch: any = {
    Page: 1,
    ItemsPerPage: 20,
    sortBy: 'FirstName',
    sortDirection: 'Ascending',
    
  }
  totalCount: number = 0;
  totalCountUser: number = 0;
  totalCountCategories: number = 0;
  ListOfCategories: Array<any> = [];
  ListOfTopices: Array<any> = [];
  ListOfUsers: Array<any> = [];
  Category = {
    id: 0
  }

  constructor(private api: ApiService, public util: UtilService, private modalService: BsModalService,) { }

  ngOnInit(): void {

    if(this.util.getCurrentUser().hasTopicsAccess) {
     // console.log('test topices')
      this.LoadListOfCategories()
     // this.LoadListOfTopices();
    }
    else {
      this.activeTab = 3;
    //  console.log('test topices 1')
    }
    
    if(this.util.getCurrentUser().isAdmin) {
      this.LoadListOfUsers()
    }
  }

  // =============================  USERS  =======================================
  LoadListOfUsers(){
    this.util.loadingStart();
    
    this.api.userList(this.UserSearch, success => {
     // console.log('load users success', success)
      this.ListOfUsers = success.list;
      this.totalCountUser = success.totalCount;
     // console.log('load users', this.ListOfUsers)
      this.util.loadingStop();
    },error => {
      this.util.loadingStop();
      this.util.openDeleteSource(error)
    })
  }

  getUserSortClass(prop) {

    if(prop == this.UserSearch.sortBy) {

      return (this.UserSearch.sortDirection == 'Ascending' ? 'fas fa-sort-down' : 'fas fa-sort-up')
    }

    return 'fas fa-sort';
  }

  UserSort(prop) {

    if(prop == this.UserSearch.sortBy) {

      this.UserSearch.sortDirection = (this.UserSearch.sortDirection == 'Ascending' ? 'Descending' : 'Ascending')
    } else {

      this.UserSearch.sortBy = prop;
      this.UserSearch.sortDirection = 'Ascending';

    }

    this.LoadListOfUsers();

  }
  
  pageChangedUsers(e) {
    if(e != this.search.Page) {
      this.search.Page = e;
      this.LoadListOfUsers();
    }
  }
  itemsPerPageChangedUsers(e) {
    this.search.Page = 1;
    this.search.ItemsPerPage = e;
  //  console.log('user Page', e)
    this.LoadListOfUsers();
  }

  openAddUserModal(delails) {
    //  console.log('details', delails)

    var delailsCopy = JSON.parse(JSON.stringify(delails));
      var modalRef = this.modalService.show(AddUserComponent, {  class: "create-modal ngDraggable", backdrop:'static', initialState: {
        user:{
  
          delailsCopy
          }
    }});
    modalRef.content.onClose.subscribe(result => {
        //console.log('result',result)
        this.LoadListOfUsers();
    })
    }

    DeleteUser(UserId){
      this.api.Deleteuser(UserId, success => {
      this.LoadListOfUsers()
      },error => {
        this.util.openDeleteSource(error.messages)
      })
    }

  // =============================  TOPICES ==================================================
  LoadListOfTopices(){
    this.util.loadingStart();
    //console.log('topice search', this.search)
    this.search.CategoryId  = this.Category.id
   this.api.TopicesList(this.search, success => {
  //  console.log('Topices', success)
    this.ListOfTopices = success.list;
    this.totalCount = success.totalCount;
    this.util.loadingStop();
   }, error => {
    this.util.loadingStop();
    this.util.openDeleteSource(error.messages)
   })
  }

  pageChanged(e) {
    if(e != this.search.Page) {
      this.search.Page = e;
      this.LoadListOfTopices();
    }
  }
  itemsPerPageChanged(e) {
    this.search.Page = 1;
    this.search.ItemsPerPage = e;
    this.LoadListOfTopices();
  }

  getSortClass(prop) {

    if(prop == this.search.sortBy) {

      return (this.search.sortDirection == 'Ascending' ? 'fas fa-sort-down' : 'fas fa-sort-up')
    }

    return 'fas fa-sort';
  }

  sort(prop) {

    if(prop == this.search.sortBy) {

      this.search.sortDirection = (this.search.sortDirection == 'Ascending' ? 'Descending' : 'Ascending')
    } else {

      this.search.sortBy = prop;
      this.search.sortDirection = 'Ascending';

    }
    this.search.Page = 1;
    this.LoadListOfTopices();

  }

  openCreateModal(delails) {
 
    var delailsCopy = JSON.parse(JSON.stringify(delails));
  
    delailsCopy.category = this.Category;
      var modalRef = this.modalService.show(UserPopupComponent, {  class: "create-modal ngDraggable", backdrop:'static', initialState: {
        Topices:{
          Categories: this.ListOfCategories,
          delailsCopy
          }
    }});
    modalRef.content.onClose.subscribe(result => {
       // console.log('result',result)
        this.LoadListOfCategories()
      this.LoadListOfTopices()
    })
    }
    
    DeleteTopice(TopiceID){
      this.api.DeleteTopice(TopiceID, success => {
        this.LoadListOfTopices()
      }, error => {
        // this.util.showError(error)
        this.util.openDeleteSource(error.messages)
      })
    }

 
  //=============================   CATEGORIES   ============================================

  LoadListOfCategories(){
    this.util.loadingStart();
    this.api.GetCategoryList(this.CategorySearch, success => {
     // console.log('ListOfCategories success',success)
    this.ListOfCategories = success.list
    this.totalCountCategories = success.totalCount;
   // console.log('ListOfCategories', this.ListOfCategories)
    this.util.loadingStop();
    }, error => {
      this.util.loadingStop();
      this.util.openDeleteSource(error.messages)
    })
  }

  openCreateCat(delails) {
    var delailsCopy = JSON.parse(JSON.stringify(delails));
    
      var modalRef = this.modalService.show(CategoryPopupComponent, {  class: "create-modal ngDraggable", backdrop:'static',  initialState: {
        NewCategory:{
  
          delailsCopy
          }
    }});
    modalRef.content.onClose.subscribe(result => {
        //console.log('result',result)
        this.LoadListOfCategories()
    })
    }

    getSortClassCategories(prop) {

      if(prop == this.CategorySearch.sortBy) {
  
        return (this.CategorySearch.sortDirection == 'Ascending' ? 'fas fa-sort-down' : 'fas fa-sort-up')
      }
  
      return 'fas fa-sort';
    }
  
    sortCategories(prop) {
  
      if(prop == this.CategorySearch.sortBy) {
  
        this.CategorySearch.sortDirection = (this.CategorySearch.sortDirection == 'Ascending' ? 'Descending' : 'Ascending')
      } else {
  
        this.CategorySearch.sortBy = prop;
        this.CategorySearch.sortDirection = 'Ascending';
  
      }
      this.CategorySearch.Page = 1;
      this.LoadListOfCategories();
  
    }

    DeleteCategory(CategoryID){
      this.api.DeleteCategory(CategoryID, success => {
        this.LoadListOfCategories()
      }, error => {
        // this.util.showError(error)
        this.util.openDeleteSource(error.messages)
      })
    }

    CategoryPageChanged(e) {
      if(e != this.CategorySearch.Page) {
        this.CategorySearch.Page = e;
        this.LoadListOfCategories();
      }
    }
    CategoryItemsPerPageChanged(e) {
      this.CategorySearch.Page = 1;
      this.CategorySearch.ItemsPerPage = e;
      this.LoadListOfCategories();
    }

    OpenTopices(Category){
      this.activeTab = 2
     // this.ListOfTopices = Category.topics
    
      this.Category = Category;
      this.LoadListOfTopices()
    }

    BackToCategories(){
      this.activeTab = 0
      this.Category = {
        id: 0
      }
    }


  // =========================   general   ====================================================

  openDeleteTopice(ID, type) {
    var modalRef =  this.modalService.show(DeleteMaamarComponent, { class: "delete-maamar-modal ngDraggable", backdrop:'static', initialState: {
      contact:{
       
         Msg: type == 'Topice' ? '?האם אתה בטוח שהנך רוצה למחוק את עניו לצמיתות': type == 'user' ?  'האם אתה בטוח שהנך רוצה למחוק את חבר לצמיתות' : 'האם אתה בטוח שהנך רוצה למחוק את קטוגרי לצמיתות',
         Del: true
      }
      
     } })

    modalRef.content.onClose.subscribe(result => {
      //console.log('result delete',result)
      if (result.success) {

        if (type == 'Topice') {
          this.DeleteTopice(ID)
        } 
        else if (type == 'user') 
         {
        //   console.log('work good')
           this.DeleteUser(ID)
         }
        
         else{
       //  console.log('ready to delete')
         this.DeleteCategory(ID)
         }
        
      }
      
  })
  }
 



  
 

 

  

 

   

  

}

enum ToggleTabs {
  categories,
  users,
  Types
}
