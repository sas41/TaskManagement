export type CommentType = "Comment" | "Reminder";
export class Comment {
    public static readonly CommentTypes: string[] = ["Comment", "Reminder"];
    constructor(
        public taskId: string,
        public type: CommentType,
        public text: string,
        public id: string | null = null,
        public creatorId: string | null = null,
        public reminderDate?: string,
        public creator?: any,
        public dateAdded?: string,
    ) { }
}