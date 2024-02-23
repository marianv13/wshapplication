import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  baseUrl! : string;
  constructor(
    public http: HttpClient) {
    this.baseUrl = "https://localhost:7003/api";
  }

  wshtransactionsAll(): Observable<any>{
    return this.http.get<any>(this.baseUrl +"/WSHTransaction")
  }

  wshtransactionsCount(): Observable<any>{
    return this.http.get<any>(this.baseUrl +"/WSHTransaction/EntityCount")
  }

  wshtransactionPost(wshtransaction: any): Observable<any> {
    console.log(wshtransaction);
    if (wshtransaction.transactionId == 0) {
      return this.http.post<any>(this.baseUrl+"/WSHTransaction", wshtransaction);
    }
    else {
      return this.http.put<any>(this.baseUrl+"/WSHTransaction/"+wshtransaction.transactionId, wshtransaction);
    }
  }

  wshtransactionById(wshtransaction: any): Observable<any> {
    console.log(wshtransaction);
    return this.http.get<any>(this.baseUrl+"/WSHTransaction/"+wshtransaction.transactionId);
  }

  wshtransactionDelete(wshtransaction: any): Observable<any> {
    console.log(wshtransaction);
    return this.http.delete<any>(this.baseUrl+"/WSHTransaction/"+wshtransaction.transactionId);
  }

}
