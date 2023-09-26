import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from 'src/interfaces/User';
import { UserLogin } from 'src/interfaces/UserLogin';
import { UserRegister } from 'src/interfaces/UserRegister';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  host = 'https://localhost:7105';
  constructor(private http: HttpClient) {}

  public register(user: UserRegister): Observable<User> {
    return this.http.post<User>(`${this.host}/api/Auth/register`, user);
  }

  public login(user: UserLogin): Observable<string> {
    return this.http.post(`${this.host}/api/Auth/login`, user, {
      responseType: 'text',
    });
  }

  public getUserWithToken(token: string): Observable<User> {
    return this.http.post<User>(`${this.host}/api/Auth/${token}`, {});
  }

  public payPremium(email: string, token: string): Observable<string> {
    const body = {
      token: token,
      email: email,
      authToken: localStorage.getItem("authToken")
    }
    let queryParams = new HttpParams();
    queryParams = queryParams.append('token', token);
    queryParams = queryParams.append('email', email);
    queryParams = queryParams.append('authToken', localStorage.getItem("authToken")?.toString()!!);
    return this.http.get(`${this.host}/api/Checkout`, {
      params: queryParams,
      responseType: 'text',
    });
  }

  public getUserWithId(id: string): Observable<User> {
    return this.http.post<User>(`${this.host}/api/Auth/user/${id}`, {});
  }
}
