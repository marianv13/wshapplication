import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { WshtransactionsComponent } from './wshtransactions/wshtransactions.component';

const routes: Routes = [
  {path:'', component: HomeComponent},
  {path:'/wshtransactions', component: WshtransactionsComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
