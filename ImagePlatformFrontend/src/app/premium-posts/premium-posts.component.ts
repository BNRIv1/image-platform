import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Post } from 'src/interfaces/Post';
import { PostsService } from 'src/services/posts.service';
import { PostDetailsComponent } from '../post-details/post-details.component';
import { CategoryService } from 'src/services/category.service';
import { Category } from 'src/interfaces/Category';

@Component({
  selector: 'app-premium-posts',
  templateUrl: './premium-posts.component.html',
  styleUrls: ['./premium-posts.component.css'],
})
export class PremiumPostsComponent implements OnInit {
  premiumPosts: Post[] = [];
  categories: Category[] = [];

  constructor(
    private postService: PostsService,
    private modalService: NgbModal,
    private categoryService: CategoryService
  ) {}
  ngOnInit(): void {
    this.postService
      .getPremiumPosts()
      .subscribe((result) => (this.premiumPosts = result));
    this.categoryService
      .getCategories()
      .subscribe((result) => (this.categories = result));
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
