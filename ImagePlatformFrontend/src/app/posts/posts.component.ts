import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Post } from 'src/interfaces/Post';
import { PostsService } from 'src/services/posts.service';
import { PostDetailsComponent } from '../post-details/post-details.component';
import { ActivatedRoute } from '@angular/router';
import { Category } from 'src/interfaces/Category';
import { CategoryService } from 'src/services/category.service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.css'],
})
export class PostsComponent implements OnInit {
  posts: Post[] = [];
  searchQuery = '';
  categoryQuery = '';
  categories: Category[] = [];

  constructor(
    private postService: PostsService,
    private modalService: NgbModal,
    private route: ActivatedRoute,
    private categoryService: CategoryService,
    public sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.searchQuery = params['search'] || null;
      this.categoryQuery = params['category'] || null;
      this.postService.getPosts(this.searchQuery, this.categoryQuery).subscribe((result) =>{
        this.posts = result;
      });
    });

    this.categoryService.getCategories().subscribe((result) => {      
      this.categories = result;
    });
    
  }

  openModal(id: string) {
    const modalRef = this.modalService.open(PostDetailsComponent, {
      scrollable: true,
      windowClass: 'myCustomModalClass',
      size: 'xl',
    });

    let data = {
      postId: id,
    };

    modalRef.componentInstance.fromParent = data;
  }
}
