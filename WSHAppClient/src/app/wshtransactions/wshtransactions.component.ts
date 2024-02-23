import { Component, OnInit, TemplateRef, ChangeDetectorRef } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ApiService } from '../api.service';
import { ActivatedRoute} from '@angular/router';
import { MatSort, Sort } from '@angular/material/sort';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { DatePipe } from '@angular/common';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';


@Component({
  selector: 'app-wshtransactions',
  templateUrl: './wshtransactions.component.html',
  styleUrls: ['./wshtransactions.component.css']
})
export class WshtransactionsComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private apiService: ApiService,
    private modalService: BsModalService,
    protected changeDetector: ChangeDetectorRef,
    private formBuilder: FormBuilder,
    private datePipe: DatePipe
  ){}

  sort! : MatSort;
  public dataSource = new MatTableDataSource<any>();
  sum = 0;
  count = 1;
  cat1 = 0;
  cat2 = 0;
  cat3 = 0;
  displayedColumns = ["id","item","sum","date",];
  displayedColumns2 = ["sumsum","count","avg"];
  formCreate?: FormGroup;
  formDelete?: FormGroup;
  modalRef?: BsModalRef;

  ngOnInit(): void {
    this.getData();
    this.createForm();
  }


  getData(): void {
    this.apiService.wshtransactionsAll().subscribe(res => {
      this.dataSource.data = res;
      console.log(this.dataSource);
    });
    this.apiService.wshTransactionSumSum().subscribe(res => {
      this.sum = res;
      console.log(this.sum);
    });
    this.apiService.wshtransactionsCount().subscribe(res => {
      this.count = res;
      console.log(this.count);
    });
    this.apiService.wshTransactionMinMax(0,999).subscribe(res => {
      this.cat1 = res;
      console.log(this.cat1);
    });
    this.apiService.wshTransactionMinMax(1000,4999).subscribe(res => {
      this.cat2 = res;
      console.log(this.cat2);
    });
    this.apiService.wshTransactionMinMax(5000,99999999).subscribe(res => {
      this.cat3 = res;
      console.log(this.cat3);
    });
  }

  createForm(): void {
    this.formCreate = this.formBuilder.group({
      transactionId : [0],
      date: [""],
      item: ["", [Validators.required,Validators.minLength(3)]],
      sum: ["", [Validators.required,Validators.min(0) ]]
    });
    this.formCreate.valueChanges.subscribe( () => {
      this.changeDetector.detectChanges();
    });
  }

  sortData(sort: Sort) {
    const data = this.dataSource.data;
    if (!sort.active || sort.direction === '') {
      this.dataSource.data = data;
      return;
    }

    this.dataSource.data = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'id':
          return compare(a.transactionId, b.transactionId, isAsc);
        case 'sum':
            return compare(a.sum, b.sum, isAsc);
        default:
          return 0;
      }
    });
  }

  create(createDialog: TemplateRef<any>){
    this.modalRef = this.modalService.show(createDialog);
  }

  createFunction(){
    console.log("Create");
    if (this.formCreate?.controls['date'].value == ""){
      this.formCreate?.controls['date'].setValue(this.datePipe.transform(new Date(), 'yyyy-MM-dd'))
    }
    this.apiService.wshtransactionPost(this.formCreate?.value).subscribe(res => {
        console.log(res);
        this.modalRef?.hide()
        this.createForm();
        this.getData();
    });
  }

}

function compare(a: number | string, b: number | string, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}

