import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';
import { MAT_DATE_LOCALE} from '@angular/material/core';
import * as _moment from 'moment';

@Component({
  selector: 'app-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.css'],
  providers: [
    ]
})
export class DatePickerComponent implements ControlValueAccessor {
  @Input() label = ""; 

  maxAge = new Date().getFullYear() - 14; 
  minDate = new Date(1920, 0, 1); 
  maxDate = new Date(this.maxAge, 0, 1)

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  }
  setDisabledState?(isDisabled: boolean): void {
  }

  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }

}
