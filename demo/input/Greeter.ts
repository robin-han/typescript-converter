namespace MyTest {
    "use strict";

    export class Greeter implements IGreeter {
        private _useName: string;
        private _greeting: string;

        constructor() {
            this._useName = '';
            this._greeting = '';
        }

        public userName(value?: string): string {
            if (arguments.length== 0) {
                return this._useName;
            } else {
                this._useName = value;
            }
        }

        public get greeting(): string {
            return this._greeting;
        }

        public set greeting(value: string) {
            this._greeting = value;
        }

        greet(): string {
            return "Hello, " + this.greeting;
        }
    }
}