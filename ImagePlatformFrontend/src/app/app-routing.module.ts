import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PostsComponent } from './posts/posts.component';
import { PostDetailsComponent } from './post-details/post-details.component';
import { UserDetailsComponent } from './user-details/user-details.component';
import { LoginComponent } from './login/login.component';
import { PurchasePremiumComponent } from './purchase-premium/purchase-premium.component';
import { AdminComponent } from './admin/admin.component';
import { PremiumPostsComponent } from './premium-posts/premium-posts.component';

const routes: Routes = [
  { path: '', component: PostsComponent },
  { path: 'posts/:id', component: PostDetailsComponent },
  { path: 'profile', component: UserDetailsComponent },
  { path: 'login', component: LoginComponent },
  { path: 'buy-premium', component: PurchasePremiumComponent },
  { path: 'admin', component: AdminComponent },
  { path: 'premium', component: PremiumPostsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
