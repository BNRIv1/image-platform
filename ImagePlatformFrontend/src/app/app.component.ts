import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { Category } from 'src/interfaces/Category';
import { PostRequest } from 'src/interfaces/PostRequest';
import { User } from 'src/interfaces/User';
import { CategoryService } from 'src/services/category.service';
import { PostsService } from 'src/services/posts.service';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  searchControl = new FormControl();
  userLoggedIn = false;
  user: User | undefined;
  categories: Category[] = [];

  postForm = new FormGroup({
    description: new FormControl('', [Validators.required]),
    image: new FormControl('', [Validators.required]),
    imageSource: new FormControl('', [Validators.required]),
    category: new FormControl('', [Validators.required]),
    isPremium: new FormControl(false, [Validators.required]),
  });

  constructor(
    private router: Router,
    private userService: UserService,
    private postService: PostsService,
    private categoryService: CategoryService,
    
  ) {}

  ngOnInit(): void {
    const token = localStorage.getItem('authToken');
    if (token) {
      this.userLoggedIn = true;
      this.userService
        .getUserWithToken(token)
        .subscribe((result) => (this.user = result));
    }

    this.categoryService
      .getCategories()
      .subscribe((result) => (this.categories = result));

    this.searchControl.valueChanges
      .pipe(debounceTime(400), distinctUntilChanged())
      .subscribe((value) => {
        const category = this.router.parseUrl(this.router.url).queryParams[
          'category'
        ];
        if (category) {
          this.router.navigate([''], {
            queryParams: { search: value, category: category },
          });
        } else {
          this.router.navigate([''], {
            queryParams: { search: value },
          });
        }
      });
  }

  submit() {
    // const data = {
    //   image: this.postForm.get('image')?.value!!,
    //   description: this.postForm.get('description')?.value!!,
    //   authorId: this.user?.id!!,
    //   categoryId: this.postForm.get('category')?.value!!,
    //   isPremium: this.postForm.get('isPremium')?.value!! ? true : false,
    // };

    const formData = new FormData();
    formData.append('Image', this.postForm.get('imageSource')?.value!!);
    formData.append('description', this.postForm.get('description')?.value!!);
    formData.append(
      'isPremium',
      this.postForm.get('isPremium')?.value!! ? 'true' : 'false'
    );
    formData.append('categoryId', this.postForm.get('category')?.value!!);
    formData.append('authorId', this.user?.id!!);
    this.postService.addPost(formData).subscribe(() => this.reloadPage());
  }

  changeCategory(e: any) {
    this.postForm.patchValue({
      category: e.target.value,
    });
  }

  changeCheckbox(e: any) {
    this.postForm.patchValue({
      isPremium: e.target.value,
    });
  }

  reloadPage() {
    const currentUrl = this.router.url;
    this.router
      .navigateByUrl('/login', { skipLocationChange: true })
      .then(() => {
        this.router.navigate([currentUrl]);
      });
  }

  onFileChange(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.postForm?.patchValue({
        imageSource: file,
      });
    }
  }

  logout() {
    localStorage.removeItem('authToken');
    window.location.href = '/';
  }

  uploadPhoto() {
    if (!this.user) {
      this.router.navigateByUrl('/login');
    }
  }
}
