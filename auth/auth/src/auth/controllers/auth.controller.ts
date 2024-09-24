import {
  Body,
  Controller,
  HttpStatus,
  Post,
  Req,
  Res,
  UnauthorizedException,
  UseGuards,
} from '@nestjs/common';
import { CreateAccountDto } from 'src/auth/dto/create-account.dto';
import { GetTokenRequestDto, LoginAccountDto } from "../dto/login-account.dto";
import { AuthService } from '../services/auth.service';
import { AuthGuard } from '../guards/auth.guard';
import { ResponseAccountDto } from '../dto/response-account.dto';
import { ApiTags } from '@nestjs/swagger';
import { Profile } from '../../profile/schemas/profile.schema';

@ApiTags('Auth')
@Controller('auth')
export class AuthController {
  constructor(private readonly authService: AuthService) {}

  @Post('signup')
  async signup(@Body() createAccountDto: CreateAccountDto): Promise<Profile> {
    return await this.authService.signup(createAccountDto);
  }

  @Post('login')
  async login(
    @Body() loginAccountDto: LoginAccountDto,
  ): Promise<ResponseAccountDto> {
    return this.authService.login(loginAccountDto);
  }

  @Post('get-token')
  async getToken(
    @Body() dto: GetTokenRequestDto,
  ): Promise<ResponseAccountDto> {
    console.log(dto);
    return this.authService.getToken(dto.userId);
  }

  @UseGuards(AuthGuard)
  @Post('protected')
  getProtectedData(@Req() req) {
    return { message: 'This route is protected', user: req.user };
  }
}
