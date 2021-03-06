// Auto-generated Code - Do Not Edit

export enum Audience {
  Children = 0,
  Teenagers = 1,
  YoungAdults = 2,
  Adults = 3
}
export class Book {
  title: string;
  author: Name;
  isAvailable: boolean;
  firstPublished: string;
  lastRevisedOn?: string;
  reviewedPositively?: boolean;
  recommendedAudience?: Audience;
}
export class Library {
  name: string;
  location: Location;
  squareMeters: number;
  levels?: number;
  allBooks: Book[];
  topBorrowed: Book[];
}
export class Location {
  latitude: number;
  longitude: number;
}
export class Name {
  firstName: string;
  title: Title;
  lastName: string;
}
export enum Title {
  Unknown = 0,
  Mr = 1,
  Mrs = 2,
  Miss = 3,
  Doctor = 4,
  Sir = 5,
  Madam = 6
}
