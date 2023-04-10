import { NgModule } from '@angular/core';
import {MatFormFieldModule, MatHint} from '@angular/material/form-field';
import {MatRadioModule} from '@angular/material/radio';
import {MatTabsModule} from '@angular/material/tabs';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';


@NgModule({
  imports: [
    MatFormFieldModule,
    MatRadioModule,
    MatTabsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule
    ],
  exports: [
    MatFormFieldModule,
    MatRadioModule,
    MatTabsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
  ],
})
export class MaterialModule {}