import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Post } from 'src/interfaces/Post';
import { PostRequest } from 'src/interfaces/PostRequest';

@Injectable({
  providedIn: 'root',
})
export class PostsService {

  host = "https://localhost:7105";

  constructor(private http: HttpClient) {}

  getPosts(search?: string, category?: string): Observable<Post[]> {
    let queryParams = new HttpParams();
    if (search && category) {
      queryParams = queryParams.append('search', search);
      queryParams = queryParams.append('category', category);
    } else if (search) {
      queryParams = queryParams.append('search', search);
    } else if (category) {
      queryParams = queryParams.append('category', category);
    }
    return this.http.get<Post[]>(`${this.host}/api/Post`, {
      params: queryParams,
    });
  }

  getPost(id: string): Observable<Post>{
    return this.http.get<Post>(`${this.host}/api/Post/${id}`)
  }

  getUserPosts(userId: string): Observable<Post[]>{
    return this.http.get<Post[]>(`${this.host}/api/Post/UserPosts/${userId}`)
  }

  addPost(request: FormData): Observable<Post>{
    return this.http.post<Post>(`${this.host}/api/Post/add`, request);
  }
  
  getPremiumPosts(): Observable<Post[]>{
    return this.http.get<Post[]>(`${this.host}/api/Post/premium`)
  }
}
