// Auto-generated Code - Do Not Edit

export enum Title {
  Unknown = 0,
  Mr = 1,
  Mrs = 2,
  Miss = 3,
  Doctor = 4,
  Sir = 5,
  Madam = 6
}
export interface Name {
  firstName: string;
  title: Title;
  lastName: string;
}
export enum Audience {
  Children = 0,
  Teenagers = 1,
  YoungAdults = 2,
  Adults = 3
}
export interface Book {
  title: string;
  author: Name;
  isAvailable: boolean;
  firstPublished: string;
  lastRevisedOn?: string;
  reviewedPositively?: boolean;
  recommendedAudience?: Audience;
}
export interface SampleApi {
  GetBook(id: number): Book;
}
