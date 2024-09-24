import { Controller, Get, Param, UseGuards } from '@nestjs/common';
import { ProfileService } from '../services/profile.service';
import { AuthGuard } from 'src/auth/guards/auth.guard';
import { ProfileDto } from '../dto/profile.dto';
import { ApiBearerAuth, ApiOperation, ApiTags } from '@nestjs/swagger';

@ApiTags('Profile')
@Controller('profile')
export class ProfileController {
  constructor(private readonly profileService: ProfileService) {}

  @UseGuards(AuthGuard)
  @ApiBearerAuth('access-token')
  @ApiOperation({ summary: 'Get profile information for authenticated user' })
  @Get(':email')
  async getProfile(@Param('email') email: string): Promise<ProfileDto> {
    return await this.profileService.getProfile(email);
  }
}
