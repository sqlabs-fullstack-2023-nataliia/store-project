import {
  CanActivate,
  ExecutionContext,
  Injectable,
  UnauthorizedException,
} from '@nestjs/common';
import { JwtService } from '@nestjs/jwt';
import { Request } from 'express';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(
    private jwtService: JwtService,
    private authService: AuthService,
  ) {}

  async canActivate(context: ExecutionContext): Promise<boolean> {
    const request = context.switchToHttp().getRequest();
    const token = this.extractTokenFromHeader(request);

    if (!token) {
      throw new UnauthorizedException('Token not found in request header');
    }

    try {
      const payload = await this.jwtService.verifyAsync(token, {
        secret: process.env.TOKEN_KEY,
      });

      const userId = payload.sub;  // Use sub as the user ID
      console.log('User ID in token:', userId);  // Log the user ID for debugging

      const profile = await this.authService.getProfile(userId);
      console.log('Profile ID in token:', profile);

      if (!profile) {
        throw new UnauthorizedException('Profile not found');
      }

      request['account'] = payload;
      request['currentProfile'] = profile;
    } catch (ex) {
      console.error('Error occurred in AuthGuard:', ex.message);
      throw new UnauthorizedException('Invalid token or unauthorized access');
    }

    return true;
  }

  // async canActivate(context: ExecutionContext): Promise<boolean> {
  //   const request = context.switchToHttp().getRequest();
  //   const token = this.extractTokenFromHeader(request);
  //
  //   if (!token) {
  //     throw new UnauthorizedException('Token not found in request header');
  //   }
  //
  //   try {
  //     // Verify JWT token
  //     const payload = await this.jwtService.verifyAsync(token, {
  //       secret: process.env.TOKEN_KEY,  // Ensure TOKEN_KEY is properly set
  //     });
  //
  //     // Fetch profile using the payload (assuming 'sub' is user ID)
  //     const profile = await this.authService.getProfile(payload.sub);
  //
  //     if (!profile) {
  //       throw new UnauthorizedException('Profile not found');
  //     }
  //
  //     // Attach user info to request for further use
  //     request['account'] = payload;
  //     request['currentProfile'] = profile;
  //   } catch (ex) {
  //     console.error('Error occurred in AuthGuard', ex.message);
  //     throw new UnauthorizedException('Invalid token or unauthorized access');
  //   }
  //
  //   return true;
  // }

  private extractTokenFromHeader(request: Request): string | undefined {
    const [type, token] = request.headers.authorization?.split(' ') ?? [];
    return type === 'Bearer' ? token : undefined;
  }
}

// import {
//   CanActivate,
//   ExecutionContext,
//   Injectable,
//   UnauthorizedException,
// } from '@nestjs/common';
// import { JwtService } from '@nestjs/jwt';
// import { Request } from 'express';
// import { AuthService } from '../services/auth.service';
//
// @Injectable()
// export class AuthGuard implements CanActivate {
//   constructor(
//     private jwtService: JwtService,
//     private authService: AuthService,
//   ) {}
//
//   async canActivate(context: ExecutionContext): Promise<boolean> {
//     const request = context.switchToHttp().getRequest();
//     const token = this.extractTokenFromHeader(request);
//     if (!token) {
//       throw new UnauthorizedException();
//     }
//     try {
//       const payload = await this.jwtService.verifyAsync(token, {
//         secret: process.env.TOKEN_KEY,
//       });
//
//       const profile = this.authService.getProfile(payload.sub);
//       if (!profile) {
//         throw new UnauthorizedException();
//       }
//
//       request['account'] = payload;
//       request['currentProfile'] = profile;
//     } catch (ex) {
//       console.log('Error occurred in AuthGuard', ex);
//       throw new UnauthorizedException();
//     }
//     return true;
//   }
//
//   private extractTokenFromHeader(request: Request): string | undefined {
//     const [type, token] = request.headers.authorization?.split(' ') ?? [];
//     return type === 'Bearer' ? token : undefined;
//   }
// }
