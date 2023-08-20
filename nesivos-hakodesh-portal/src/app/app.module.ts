import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

//third party
import { SweetAlert2Module } from "@sweetalert2/ngx-sweetalert2";
import { CookieModule } from "ngx-cookie";
import { NgxSpinnerModule } from 'ngx-spinner';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { NgSelectModule } from '@ng-select/ng-select';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { VirtualScrollerModule } from 'ngx-virtual-scroller';
import { ToastrModule } from 'ngx-toastr';



//local
import { AppComponent } from './app.component';
import { RoutingModule } from './modules/routing.module';
import { AuthComponent } from './components/auth/auth.component';
import { MainComponent } from './components/main/main.component';
import { MaamarimListComponent } from './components/main/maamarim/maamarim-list/maamarim-list.component';
import { MaamarimDetailComponent } from './components/main/maamarim/maamarim-detail/maamarim-detail.component';
import { NewMaamarPopupComponent } from './components/main/maamarim/new-maamar-popup/new-maamar-popup.component';
import { UserListComponent } from './components/main/users/user-list/user-list.component';
import { UserPopupComponent } from './components/main/users/user-popup/user-popup.component';
import { SafeHtmlPipe, SafePipe } from './services/safe.pipe';
import { TestComponent } from './components/main/test/test.component';
import { SharedModule } from './shared/shared.module';
import { DeleteMaamarComponent } from './components/main/maamarim/delete-maamar/delete-maamar.component';
import { TorahsListComponent } from './components/main/torahs/torahs-list/torahs-list.component';
import { TorahsDetailsComponent } from './components/main/torahs/torahs-details/torahs-details.component';
import { TorahsNewPopupComponent } from './components/main/torahs/torahs-new-popup/torahs-new-popup.component';
import { SefurimListComponent } from './components/main/sefurim/sefurim-list/sefurim-list.component';
import { SefurimDetailsPopupComponent } from './components/main/sefurim/sefurim-details-popup/sefurim-details-popup.component';
import { ProjectsListComponent } from './components/main/projects/projects-list/projects-list.component';
import { ProjectsDetailsComponent } from './components/main/projects/projects-details/projects-details.component';
import { SourcesListComponent } from './components/main/sources/sources-list/sources-list.component';
import { SourcesDetailsPopupComponent } from './components/main/sources/sources-details-popup/sources-details-popup.component';
import { MergePopupComponent } from './components/main/maamarim/merge-popup/merge-popup.component';
import { SearchResultsComponent } from './components/main/search-results/search-results.component';
import { MergeParagraphPopupComponent } from './components/main/maamarim/merge-paragraph-popup/merge-paragraph-popup.component';
import { MergeMaamarPopUpComponent } from './components/main/torahs/merge-maamar-pop-up/merge-maamar-pop-up.component';
import { AddUserComponent } from './components/main/users/add-user/add-user.component';
import { SaferPdfComponent } from './components/main/sefurim/safer-pdf/safer-pdf.component';
import { LinkMaamarComponent } from './components/main/sefurim/link-maamar/link-maamar.component';
import { CanGoBackGuard } from './modules/can-go-back.guard';
import { LibraySearchComponent } from './components/main/maamarim/libray-search/libray-search.component';
import { LibraryDetailComponent } from './components/main/Library/Library-details/library-detail/library-detail.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { LibraryListComponent } from './components/main/Library/Library-list/library-list/library-list.component';
import { CategoryPopupComponent } from './components/main/users/Category-PopUp/Category-popup.component';
import { AngularDraggableModule } from 'angular2-draggable';


@NgModule({
  declarations: [
    AppComponent,
    SafePipe,
    SafeHtmlPipe,
    AuthComponent,
    MainComponent,
    MaamarimListComponent,
    MaamarimDetailComponent,
    NewMaamarPopupComponent,
    UserListComponent,
    UserPopupComponent,
    TestComponent,
    DeleteMaamarComponent,
    TorahsListComponent,
    TorahsDetailsComponent,
    TorahsNewPopupComponent,
    SefurimListComponent,
    SefurimDetailsPopupComponent,
    ProjectsListComponent,
    ProjectsDetailsComponent,
    SourcesListComponent,
    SourcesDetailsPopupComponent,
    MergePopupComponent,
    SearchResultsComponent,
    MergeParagraphPopupComponent,
    MergeMaamarPopUpComponent,
    AddUserComponent,
    SaferPdfComponent,
    LinkMaamarComponent,
    LibraySearchComponent,
    LibraryDetailComponent,
    LibraryListComponent,
    CategoryPopupComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    RoutingModule,
    HttpClientModule,
    SweetAlert2Module.forRoot(),
    CookieModule.forRoot(),
    NgxSpinnerModule,
    SharedModule,
    BrowserAnimationsModule,
    BsDropdownModule.forRoot(),
    TooltipModule.forRoot(),
    ModalModule.forRoot(),
    NgSelectModule,
    FormsModule,
    TypeaheadModule.forRoot(),
    BsDatepickerModule.forRoot(),
    PopoverModule.forRoot(),
    NgbModule,
    CKEditorModule,
    VirtualScrollerModule,
    InfiniteScrollModule,
    AngularDraggableModule,
    ToastrModule.forRoot({
      timeOut: 1000,
      positionClass: 'toast-bottom-left',
      preventDuplicates: true,
      
    }),
  ],
  providers: [CanGoBackGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
