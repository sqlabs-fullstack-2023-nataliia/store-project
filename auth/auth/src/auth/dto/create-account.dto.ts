import { IsEmail } from 'class-validator';
import { ApiProperty } from '@nestjs/swagger';

export class CreateAccountDto {
  @ApiProperty({ required: true })
  @IsEmail()
  email: string;
  // @Length(6, 50, {
  //   message: 'Password length Must be between 6 and 50 characters',
  // })
  @ApiProperty({ required: true })
  password: string;

  @ApiProperty({ required: false })
  firstName: string;

  @ApiProperty({ required: false })
  lastName: string;
}
