import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from 'src/interfaces/Category';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  host = 'https://localhost:7105';

  constructor(private http: HttpClient) {}

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.host}/api/Category`);
  }

  deleteCategory(id: string): Observable<string> {
    return this.http.delete(`${this.host}/api/Category/delete/${id}`, {
      responseType: 'text',
    });
  }

  addCategory(data: any): Observable<Category> {
    return this.http.post<Category>(`${this.host}/api/Category/add`, data);
  }

  updateCategory(category: Category): Observable<Category> {
    return this.http.post<Category>(
      `${this.host}/api/Category/update`,
      category
    );
  }
}
