import { Routes, RouterModule } from "@angular/router";
import { NgModule } from '@angular/core';
import { AuthComponent } from '../components/auth/auth.component';
import { MaamarimListComponent } from '../components/main/maamarim/maamarim-list/maamarim-list.component';
import { MaamarimDetailComponent } from '../components/main/maamarim/maamarim-detail/maamarim-detail.component';
import { UserListComponent } from '../components/main/users/user-list/user-list.component';
import { MainComponent } from '../components/main/main.component';
import { TestComponent } from '../components/main/test/test.component';
import { TorahsListComponent } from '../components/main/torahs/torahs-list/torahs-list.component';
import { TorahsDetailsComponent } from '../components/main/torahs/torahs-details/torahs-details.component';
import { SefurimListComponent } from '../components/main/sefurim/sefurim-list/sefurim-list.component';
import { ProjectsListComponent } from '../components/main/projects/projects-list/projects-list.component';
import { ProjectsDetailsComponent } from '../components/main/projects/projects-details/projects-details.component';
import { SourcesListComponent } from '../components/main/sources/sources-list/sources-list.component';
import { AuthenticatedUserGuard } from './authenticated-user.guard';
import { SearchResultsComponent } from '../components/main/search-results/search-results.component';
import { SaferPdfComponent } from "../components/main/sefurim/safer-pdf/safer-pdf.component";
import { CanGoBackGuard } from "./can-go-back.guard";
import { LibraryDetailComponent } from "../components/main/Library/Library-details/library-detail/library-detail.component";
import { LibraryListComponent } from "../components/main/Library/Library-list/library-list/library-list.component";


const routes: Routes = [

    {
        path: 'login',
        component: AuthComponent
    },
    {
        path: '',
        component: MainComponent,
        canActivate: [AuthenticatedUserGuard],
        children: [
            {
                path: '',
                redirectTo: 'maamarim',
                pathMatch: 'full'
            },
            {
                path: 'maamarim',
                children: [
                    {
                        path: '',
                        component: MaamarimListComponent
                    },
                    {
                        path: ':id',
                        component: MaamarimDetailComponent,
                        canDeactivate: [CanGoBackGuard]
                    }
                ]
            },
            {
                path: 'torahs',
                children: [
                    {
                        path: '',
                        component: TorahsListComponent
                    },
                    {
                        path: ':id',
                        component: TorahsDetailsComponent
                    }
                ]
            },
            {
                path: 'sefurim',
                children: [
                    {
                        path: '',
                        component: SefurimListComponent
                    },
                    {
                        path: ':id',
                        component: SaferPdfComponent,
                        canDeactivate: [CanGoBackGuard]
                    }
                ]
            },
            {
                path: 'library',
                children: [
                    {
                        path: '',
                        component: LibraryListComponent
                    },
                    {
                        path: ':id',
                        component: LibraryDetailComponent,
                       // canDeactivate: [CanGoBackGuard]
                    }
                ]
            },
            {
                path: 'projects',
                children: [
                    {
                        path: '',
                        component: ProjectsListComponent
                    },
                    {
                        path: ':id',
                        component: ProjectsDetailsComponent
                    }
                ]
            },
            {
                path: 'sources',
                component: SourcesListComponent
            },
            {
                path: 'settings',
                children: [
                    {
                        path: 'users',
                        component: UserListComponent
                    },
                ]
            },
            {
                path: 'test',
                component: TestComponent
            },
            {
                path: 'search-results',
                component: SearchResultsComponent
            }
        ]
    }
]


@NgModule({
    imports: [RouterModule.forRoot(routes, {useHash: true})],
    exports: [RouterModule]
})
export class RoutingModule { }
