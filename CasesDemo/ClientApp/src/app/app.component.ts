import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  
  title = 'Case Statistics';
  caseFormGroup : FormGroup;
  isStateValid:boolean = true;
  isDateValid:boolean = true;
  isNewCasesValid:boolean = true;
  isRecoveryValid:boolean = true;
  isDeathValid:boolean = true;
  fromDate = new Date();
  toDate= new Date();
  casesDatasource:any =[];
  stateDatasource: any = [{ "key": '0', "value": "Assam" }, { "key": '1', "value": "Tamil Nadu" }, { "key": '2', "value": "Andhra Pradesh" }, { "key": '3', "value": "Kerala" }, { "key": '4', "value": "Karnataka" },
  { "key": '5', "value": "Odisha" }, { "key": '6', "value": "West Bengal" }, { "key": '7', "value": "Maharastra" }, { "key": '8', "value": "Rajasthan" }, { "key": '9', "value": "Gujarath" }];

  constructor(private _fb:FormBuilder, private http: HttpClient, private datePipe : DatePipe){
    this.buildForm();
  }

  ngOnInit(): void {
    this.getCases();
  }

  buildForm(){
    this.caseFormGroup = this._fb.group({
      id:new FormControl(0),
      state : new FormControl('', Validators.required),
      date : new FormControl('', Validators.required),
      newCases : new FormControl(0,Validators.required),
      recovery : new FormControl(0,Validators.required),
      death : new FormControl(0,Validators.required),
    });
  }

  getCases(){
    var obj = {fromDate:this.datePipe.transform(this.fromDate, 'dd/MM/yyyy'),
    toDate:this.datePipe.transform(this.toDate, 'dd/MM/yyyy')};
    this.http.post("api/getCases", obj ).subscribe(data => {
      this.casesDatasource = data["result"];
    });
  }

  updateCase(){
    this.caseFormGroup.updateValueAndValidity();
    if(this.caseFormGroup.valid){
      this.isDateValid = this.caseFormGroup.controls["date"].value != "" ? true : false;
      var obj = {
        id:this.caseFormGroup.controls['id'].value,
      state : this.stateDatasource[this.caseFormGroup.controls['state'].value]['value'],
      date : this.datePipe.transform(this.caseFormGroup.controls['date'].value, 'dd/MM/yyyy'),
      newCases : this.caseFormGroup.controls['newCases'].value,
      recovery : this.caseFormGroup.controls['recovery'].value,
      death :this.caseFormGroup.controls['death'].value
      };
      this.http.post("api/updateCases", obj).subscribe(data => {
        this.caseFormGroup.reset();
        this.buildForm();
        this.isDateValid = true;
        this.getCases();
      });
    }else{
      this.isStateValid = this.caseFormGroup.controls["state"].value != "" ? true : false;
      this.isDateValid = this.caseFormGroup.controls["date"].value != "" ? true : false;
      this.isNewCasesValid = this.caseFormGroup.controls["newCases"].value != "" || this.caseFormGroup.controls["newCases"].value == 0 ? true : false;
      this.isRecoveryValid = this.caseFormGroup.controls["recovery"].value != "" || this.caseFormGroup.controls["recovery"].value == 0 ? true : false;
      this.isDeathValid = this.caseFormGroup.controls["death"].value != "" || this.caseFormGroup.controls["death"].value == 0 ? true : false;
    }
  }

  stateChanged(event){
    this.isStateValid = true;
  }

  onDateChanged(event){
    if(event.name == "value"){
      this.isDateValid = this.caseFormGroup.controls["date"].value != "" ? true : false;
    }
  }

  statusDateChanged(event){
    if(event.name == "value" && event.value){
      
      setTimeout( () => {
        this.getCases();
      }, 300);
      
      }
  }
  

  
}
