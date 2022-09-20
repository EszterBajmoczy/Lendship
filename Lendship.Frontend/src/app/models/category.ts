export interface Category {
  id: number,
  name: string
}

export class Category{
  id: number;
  name: string;

  constructor(id: number, name: string) {
    this.id = id;
    this.name = name;
  }
}
