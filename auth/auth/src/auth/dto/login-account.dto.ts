import { ApiProperty } from '@nestjs/swagger';

export class LoginAccountDto {
  @ApiProperty({ required: true })
  email: string;

  @ApiProperty({ required: true })
  password: string;
}

export class GetTokenRequestDto {
  @ApiProperty({ required: true })
  userId: string
}


