import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ApiService } from '../api.service';
import { ActivatedRoute} from '@angular/router';
import { MatSort, Sort } from '@angular/material/sort';


@Component({
  selector: 'app-wshtransactions',
  templateUrl: './wshtransactions.component.html',
  styleUrls: ['./wshtransactions.component.css']
})
export class WshtransactionsComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private apiService: ApiService
  ){}

  sort! : MatSort;
  public dataSource = new MatTableDataSource<any>();
  displayedColumns = ["id","date","item","sum"];

  ngOnInit(): void {
    this.getData();
  }

  getData(): void {
    this.apiService.wshtransactionsAll().subscribe(res => {
      this.dataSource.data = res;
      console.log(this.dataSource);
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
}

function compare(a: number | string, b: number | string, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}

