import { Photo } from "./photo"

export interface Member {
  id: number
  userName: string
  age: number
  photoUrl: string
  dateOfBirth: string
  knownAs: string
  createdAt: Date
  lastActive: Date
  gender: string
  aboutSection: string
  interests: string
  lookingFor: string
  city: string
  country: string
  photos: Photo[]
}

