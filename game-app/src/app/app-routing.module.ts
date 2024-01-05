import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthComponentComponent } from './components/auth-component/auth-component.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { DashboardAdminComponent } from './components/dashboard-admin/dashboard-admin.component';
import { GameComponentComponent } from './components/game-component/game-component.component';

const routes: Routes = [
  { path: "", redirectTo: 'login', pathMatch: 'full'},
  { path: 'login', component: AuthComponentComponent },
  { path: 'dashboard', component: DashboardComponent},
  { path: 'dashboard-admin', component: DashboardAdminComponent},
  { path: 'play', component: GameComponentComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
