import { Category } from "./Category";
import { User } from "./User";

export interface Post{
    imageUrl: string,
    datePublished: Date,
    views: number,
    downloads: number,
    description: string,
    id: string,
    isPremium: boolean,
    author: User,
    category: Category
}