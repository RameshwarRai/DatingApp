import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './members/lists/lists.component';
import { MemberDeatilComponent } from './members/member-deatil/member-deatil.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './members/messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
{path:'',component:HomeComponent},
{
  path:'',
  runGuardsAndResolvers:'always',
  canActivate:[AuthGuard],
  children:[
    {path:'members',component:MemberListComponent,canActivate :[AuthGuard]},
    {path:'members/:id',component:MemberDeatilComponent},
    {path:'lists',component:ListsComponent	},
    {path:'messages',component:MessagesComponent},
    {path:'**',component:HomeComponent,pathMatch:"full"}
  ]
}


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
