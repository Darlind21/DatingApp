import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { inject, Injectable, model, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/photo';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  baseUrl = environment.apiUrl;
  paginatedResult = signal<PaginatedResult<Member[]> | null> (null);
  memberCache = new Map ();
  user = this.accountService.currentUser();
  userParams = signal<UserParams>(new UserParams(this.user));

  resetUserParams(){
    this.userParams.set(new UserParams(this.user));
  }

  getMembers(){
    const response = this.memberCache.get(Object.values(this.userParams()).join('-'))

    if (response) return setPaginatedResponse(response, this.paginatedResult);

    let params = setPaginationHeaders(this.userParams().pageNumber, this.userParams().pageSize);

    params = params.append('minAge', this.userParams().minAge);
    params = params.append('maxAge', this.userParams().maxAge);
    params = params.append('gender', this.userParams().gender);
    params = params.append('orderBy', this.userParams().orderBy)

    return this.http.get<Member[]>(this.baseUrl + 'users', {observe: 'response', params}).subscribe({
      next: response => {
        setPaginatedResponse(response, this.paginatedResult);
        this.memberCache.set(Object.values(this.userParams()).join('-'), response)
      }
    })
  }

  // Fixed method name and logic
  getMember(username: string){
    console.log('getMember called with username:', username);
    console.log('Current cache size:', this.memberCache.size);
    
    // Check if we have any cached data
    if (this.memberCache.size > 0) {
      console.log('Searching in cache...');
      
      // Fixed cache search logic
      const member: Member | undefined = [...this.memberCache.values()]
        .reduce((arr: Member[], elem: any) => {
          // Handle both direct arrays and response objects with body property
          const members = elem.body ? elem.body : elem;
          return arr.concat(Array.isArray(members) ? members : []);
        }, [])
        .find((m: Member) => m.userName === username);

      console.log('Found member in cache:', member ? 'Yes' : 'No');
      
      if (member) {
        console.log('Returning cached member:', member.knownAs);
        return of(member);
      }
    }

    console.log('Making API call to:', this.baseUrl + 'users/' + username);
    return this.http.get<Member>(this.baseUrl + 'users/' + username).pipe(
      tap(member => {
        console.log('API returned member:', member ? member.knownAs : 'null');
      })
    );
  }

  // Keep the old method name for backward compatibility, but call the fixed one
  getMemeber(username: string) {
    console.warn('getMemeber is deprecated, use getMember instead');
    return this.getMember(username);
  }

  updateMember(member: Member){
    return this.http.put(this.baseUrl + 'users', member).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(
      //     m => m.userName === member.userName ? member : m
      //   ))
      // })
    )
  }

  setMainPhoto(photo: Photo) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photo.id, {}).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => {
      //     if (m.photos.includes(photo)) {
      //       m.photoUrl = photo.url
      //     }
      //     return m;
      //   }))
      // })
    )
  }

  deletePhoto(photo: Photo) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photo.id).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => {
      //     if (m.photos.includes(photo)) {
      //       m.photos = m.photos.filter(x => x.id !== photo.id)
      //     }
      //     return m
      //   }))
      // })
    )
  }
}