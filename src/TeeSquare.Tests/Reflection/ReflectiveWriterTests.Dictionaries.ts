// Auto-generated Code - Do Not Edit

export enum Genre {
  Drama = 0,
  Doco = 1
}
export interface Indexes {
  moviesByYear: { [key: number]: Movie[] };
  moviesByDirector: { [key: string]: Movie[] };
  moviesByGenre: Record<Genre, Movie[]>;
}
export interface Movie {
  name: string;
  year: number;
  director: string;
  genre: Genre;
}
