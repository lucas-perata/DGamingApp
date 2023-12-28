import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule} from '@angular/common/http';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import {NgxSpinnerModule } from 'ngx-spinner';
import { FileUploadModule } from 'ng2-file-upload';
import { TimeagoModule } from 'ngx-timeago';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    TimeagoModule.forRoot(),
    NgxSpinnerModule.forRoot({
      type: "ball-fussion",
    }      
    ),
    FileUploadModule,
    [ HttpClientModule, NgxGalleryModule ],
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    })
  ],
  exports: [
    NgxSpinnerModule,
    FileUploadModule,
    TimeagoModule
  ]
})
export class SharedModule { }
