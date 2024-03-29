export type TaskStatus = "New" | "InProgress" | "Done";
export type TaskType = "Task" | "Bug";
export class Task {
    public static readonly TaskStatuses: string[] = ["New", "InProgress", "Done"];
    public static readonly TaskTypes: string[] = ["Task", "Bug"];
    constructor(
        public title: string,
        public description: string,
        public requiredBy: string,
        public status: TaskStatus,
        public type: TaskType,
        public id: string | null = null,
        public creatorId: string | null = null,
        public creator?: any,
        public dateAdded?: string,
        public assignees?: any[],
        public comments?: any[],
        public nextActionDate?: string,
    ) { }

    static get Placeholder() {
        return new Task(
            'Title',
            'Description',
            '1990-01-01',
            'New',
            'Task',
            'id',
            'creatorId',
            { id:'cid', displayName: 'Creator' },
            '1990-01-01',
            [],
            [],
            '1990-01-01'
        );
    }
}