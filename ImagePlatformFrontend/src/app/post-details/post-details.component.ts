import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Post } from 'src/interfaces/Post';
import { User } from 'src/interfaces/User';
import { PostsService } from 'src/services/posts.service';
import { UserService } from 'src/services/user.service';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-post-details',
  templateUrl: './post-details.component.html',
  styleUrls: ['./post-details.component.css'],
})
export class PostDetailsComponent implements OnInit {
  post: Post | undefined;
  currentUser: User | undefined;
  @Input() fromParent: any;

  constructor(
    private postService: PostsService,
    private route: ActivatedRoute,
    public activeModal: NgbActiveModal,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.postService
      .getPost(this.fromParent.postId)
      .subscribe((result) => (this.post = result));

    this.userService
      .getUserWithToken(localStorage.getItem('authToken')!!)
      .subscribe((result) => (this.currentUser = result));
  }

  closeModal() {
    this.activeModal.close();
  }

  downloadImg(url: string) {
    var link = document.createElement('a');
    link.download = 'image.jpg';
    link.href = `https://localhost:7105/${url}`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    // link = "";
    // link = null;
  }
}
