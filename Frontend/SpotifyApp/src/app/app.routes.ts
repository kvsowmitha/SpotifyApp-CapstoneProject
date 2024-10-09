import { Routes } from '@angular/router';
import { NavBarComponent } from './component/nav-bar/nav-bar.component';
import { SigninComponent } from './component/signin/signin.component';
import { MusicPlayerComponent } from './component/music-player/music-player.component';
import { MusicDisplayComponent } from './component/music-display/music-display.component';
import { ForgetPasswordComponent } from './component/forget-password/forget-password.component';
import { ResetPasswordComponent } from './component/reset-password/reset-password.component';
import { ViewProfileComponent } from './component/view-profile/view-profile.component';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: MusicDisplayComponent },
  { path: 'album', component: MusicDisplayComponent },
  { path: 'artist', component: MusicDisplayComponent },
  { path: 'favorite', component: MusicDisplayComponent },
  { path: 'forget-password', component: ForgetPasswordComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { path: 'profile', component:   ViewProfileComponent},
];
