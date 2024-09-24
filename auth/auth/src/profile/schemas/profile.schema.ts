import mongoose, { Document, HydratedDocument, Schema as MongooseSchema } from "mongoose";
import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { Account } from '../../auth/schemas/account.schema';

export type ProfileDocument = HydratedDocument<Profile>;

@Schema()
export class Profile extends Document{

  @Prop({ type: MongooseSchema.Types.ObjectId })
  id: MongooseSchema.Types.ObjectId;

  @Prop({ required: false })
  firstName: string;

  @Prop({ required: false })
  lastName: string;

  @Prop({ type: MongooseSchema.Types.ObjectId, ref: 'Account', required: true })
  account: MongooseSchema.Types.ObjectId;
  // @Prop({ type: MongooseSchema.Types.ObjectId, ref: 'Account', required: true })
  // account: MongooseSchema.Types.ObjectId | string;
}

export const ProfileSchema = SchemaFactory.createForClass(Profile);
