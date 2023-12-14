import { User } from "./user";

export class UserParams {
    join(arg0: string): { [s: string]: unknown; } | ArrayLike<unknown> {
      throw new Error('Method not implemented.');
    }
    gender: string; 
    minAge = 18; 
    maxAge = 99; 
    pageNumber = 1; 
    pageSize = 2; 
    orderBy: string; 

    constructor(user: User) {
        this.gender = user.gender === 'female' ? 'male' : 'female'; 
        this.orderBy = user.orderBy === 'LastActive' ? 'Created' : 'LastActive'; 
    }
}