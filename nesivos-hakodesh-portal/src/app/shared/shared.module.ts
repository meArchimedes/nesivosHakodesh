import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import { HeaderComponent } from './header/header.component';
import { PaginationComponent } from './pagination/pagination.component';
import { UserSettingsComponent } from './user-settings/user-settings.component';
import { RoutingModule } from '../modules/routing.module';
import { GlobalSearchComponent } from './global-search/global-search.component';
import { HebrewDatapickerComponent } from './hebrew-datapicker/hebrew-datapicker.component';
import { AutofocusDirective } from './custom-directives';
import { PdfViewComponent } from './pdf-view/pdf-view.component';

import { PdfViewerModule } from 'ng2-pdf-viewer';
import { PdfViewTempComponent } from './pdf-view-temp/pdf-view-temp.component';
import { PdfViewNewComponent } from './pdf-view-new/pdf-view-new.component';

import { NgxExtendedPdfViewerModule } from './lib/ngx-extended-pdf-viewer.module';
//import { NgxExtendedPdfViewerModule } from 'ngx-extended-pdf-viewer';


@NgModule({
  declarations: [
    HeaderComponent,
    PaginationComponent,
    UserSettingsComponent,
    GlobalSearchComponent,
    HebrewDatapickerComponent,
    AutofocusDirective,
    PdfViewComponent,
    PdfViewTempComponent,
    PdfViewNewComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    BrowserAnimationsModule,
    BsDropdownModule.forRoot(),
    RoutingModule,
    BsDatepickerModule.forRoot(),
    NgbModule,
    PdfViewerModule,
    NgxExtendedPdfViewerModule,
  ],
  exports: [
    HeaderComponent,
    PaginationComponent,
    UserSettingsComponent,
    GlobalSearchComponent,
    HebrewDatapickerComponent,
    AutofocusDirective,
    PdfViewComponent,
    PdfViewTempComponent,
    PdfViewNewComponent,
  ]
})
export class SharedModule { }
