import { Account } from '../../auth/schemas/account.schema';

export class ProfileDto {
  firstName?: string;
  lastName?: string;
  account?: string;

  constructor(firstName: string, lastName: string, account: string) {
    this.firstName = firstName;
    this.lastName = lastName;
    this.account = account;
  }
}
