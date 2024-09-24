import { InjectModel } from '@nestjs/mongoose';
import { Model } from 'mongoose';
import {
  forwardRef,
  Inject,
  Injectable,
  InternalServerErrorException,
  UnauthorizedException,
} from '@nestjs/common';
import { ProfileDto } from '../dto/profile.dto';
import { Profile } from '../schemas/profile.schema';
import { AuthService } from '../../auth/services/auth.service';

@Injectable()
export class ProfileService {
  constructor(
    @InjectModel(Profile.name) private profileModel: Model<Profile>,
    @Inject(forwardRef(() => AuthService))
    private authService: AuthService,
  ) {}

  async getProfile(email: string): Promise<ProfileDto> {
    const account = await this.authService.findOneByEmail(email);
    if (!account) {
      throw new UnauthorizedException('Account not found');
    }

    const profile = await this.profileModel
      .findOne({ account: account._id })
      .exec();
    if (!profile) {
      throw new UnauthorizedException('Profile not found');
    }

    await profile.populate('account');

    return profile.toObject();
  }

  async create(profileDto: ProfileDto): Promise<Profile> {
    try {
      const profile = await this.profileModel.create(profileDto);

      await profile.populate({
        path: 'account',
        select: '-password',
      });

      return profile.toObject();
    } catch (error) {
      console.error('Error creating profile:', error);
      throw new InternalServerErrorException('Failed to create profile');
    }
  }
}
