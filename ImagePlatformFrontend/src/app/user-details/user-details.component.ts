import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin } from 'rxjs';
import { Post } from 'src/interfaces/Post';
import { User } from 'src/interfaces/User';
import { PostsService } from 'src/services/posts.service';
import { UserService } from 'src/services/user.service';
import { PostDetailsComponent } from '../post-details/post-details.component';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css'],
})
export class UserDetailsComponent implements OnInit {
  userPosts: Post[] = [];
  user: User | undefined;
  authorId: string | undefined;

  constructor(
    private postService: PostsService,
    private userService: UserService,
    private router: Router,
    private modalService: NgbModal,
  ) {}

  ngOnInit(): void {
    this.authorId = this.router.parseUrl(this.router.url).queryParams[
      'authorId'
    ];
    this.userService
      .getUserWithId(this.authorId!!)
      .subscribe((result) => (this.user = result));

    this.postService
      .getUserPosts(this.authorId!!)
      .subscribe((result) => (this.userPosts = result));
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
