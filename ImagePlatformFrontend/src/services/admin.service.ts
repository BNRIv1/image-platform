import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  host = 'https://localhost:7105';
  constructor(private http: HttpClient) { }

  exportUsers(): Observable<any>{
    return this.http.get(`${this.host}/api/Admin/users`, {
      responseType: 'blob'
    })
  }
}
