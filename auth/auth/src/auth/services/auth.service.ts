import { CreateAccountDto } from 'src/auth/dto/create-account.dto';
import { LoginAccountDto } from '../dto/login-account.dto';
import { InjectModel } from '@nestjs/mongoose';
import { Account } from 'src/auth/schemas/account.schema';
import { Model } from 'mongoose';
import {
  BadRequestException,
  forwardRef,
  Inject,
  Injectable,
  UnauthorizedException,
} from '@nestjs/common';
import * as bcrypt from 'bcrypt';
import { JwtService } from '@nestjs/jwt';
import { ResponseAccountDto } from '../dto/response-account.dto';
import { ProfileService } from '../../profile/services/profile.service';
import { Profile } from '../../profile/schemas/profile.schema';
import { ProfileDto } from "../../profile/dto/profile.dto";

@Injectable()
export class AuthService {
  constructor(
    @InjectModel(Account.name) private accountModel: Model<Account>,
    private jwtService: JwtService,
    @Inject(forwardRef(() => ProfileService))
    private profileService: ProfileService,
  ) {}

  async signup(createAccountDto: CreateAccountDto): Promise<Profile> {
    let createdAccount = null;
    const existAccount = await this.accountModel.findOne({
      email: createAccountDto.email,
    });

    if (existAccount) {
      throw new BadRequestException(
        `Account with email: ${createAccountDto.email} already exist`,
      );
    }

    const hash = await bcrypt.hash(createAccountDto.password, 10);
    createdAccount = new this.accountModel({
      ...createAccountDto,
      password: hash,
    });
    await createdAccount.save();

    return await this.profileService.create({
      ...createAccountDto,
      account: createdAccount._id,
    });
  }

  async login(loginAccountDto: LoginAccountDto): Promise<ResponseAccountDto> {
    const account = await this.accountModel.findOne({
      email: loginAccountDto.email,
    });
    if (!account) {
      throw new UnauthorizedException();
    }
    const passwordMatch = await bcrypt.compare(
      loginAccountDto.password,
      account.password,
    );
    if (!passwordMatch) {
      throw new UnauthorizedException();
    }
    const payload = {
      sub: account._id,
      aud: 'test',
      iss: 'test',
    };
    const access_token = await this.jwtService.signAsync(payload);
    return {
      access_token: access_token,
      email: loginAccountDto.email,
    };
  }

  // async getProfile(userId: string): Promise<Account> {
  //   const account = await this.accountModel.findById(userId).exec();
  //   if (!account) {
  //     throw new UnauthorizedException();
  //   }
  //
  //   return account.toObject();
  // }
  async getProfile(userId: string): Promise<ProfileDto> {
    const account = (
      await this.accountModel.findById(userId).exec()
    ).toObject();
    if (!account) {
      throw new UnauthorizedException('Account not found');
    }

    console.log('Account found:', account);

    // Find the profile by matching the account ObjectId
    return await this.profileService.getProfile(account.email);
  }

  async findOneByEmail(email: string): Promise<Account> {
    const account = await this.accountModel.findOne({ email }).exec();

    if (!account) {
      throw new UnauthorizedException('Account not found');
    }

    return account.toObject();
  }

  async getToken(userId: string): Promise<ResponseAccountDto> {
    const account = await this.accountModel.findById(userId);
    if (!account) {
      throw new UnauthorizedException();
    }

    const payload = {
      sub: account._id,
      aud: 'test',
      iss: 'test',
    };
    const access_token = await this.jwtService.signAsync(payload);
    return {
      access_token: access_token,
      email: account.email,
    };
  }
}
