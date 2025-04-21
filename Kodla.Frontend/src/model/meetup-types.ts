export type Meetup = {
  meetupId: number;
  name: string;
  description: string;
  date: string;
};

export type RequestAttendeeResponse = {
  message: string;
  requestId: string;
}

export type AttendeeRequestStatus = {
  requestId: string;
  status: string;
}
