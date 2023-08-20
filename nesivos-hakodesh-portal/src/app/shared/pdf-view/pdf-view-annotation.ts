export interface PdfViewAnnotation {

    id: number;
    pageNumber: number;
    shapeType: ShapeType;
    customData: any;

    getShapeType();
    Draw(canvas); 
    IsOnBorder(e);
    IsOnMovingSpot(e);
    GetMouseMovingType(e);
    Move(e);
    Select(canvas);
    isBigEnough();
    FixBeforeSave()
    IsChanged(backup);

}

export class PdfViewRectangle implements PdfViewAnnotation {
    
    id: number;
    pageNumber: number;
    shapeType: ShapeType;
    customData: any = {};

    x: number;
    y: number;
    h: number;
    w: number;
    //handlesSize = 5;
    scale: number = 1;

    private movingType: string = '';
    private prevPoint: any = null;

    constructor() {
        
        this.id = Math.floor(Math.random() * 9999) * -1;
        this.shapeType = ShapeType.Rectangle;
    }

    getX() {return this.x * this.scale;}
    getY() {return this.y * this.scale;}
    getW() {return this.w * this.scale;}
    getH() {return this.h * this.scale;}
    getHandleSize() {return 5 * this.scale;}

    getShapeType() {
        return this.shapeType;
    }

    Draw(pdfView) {

        this.scale = pdfView.scale;

        //console.log('scale', this.scale);
        //console.log('X', this.getX());

        var cx = pdfView.canvas.getContext('2d');
        cx.beginPath();
        cx.lineWidth = this.GetLineWidth();
        cx.strokeStyle = "#ffcd45";
        cx.rect(this.getX(), this.getY(), this.getW(), this.getH());
        cx.stroke();

    }

    IsOnBorder(e) {

        var mPoint = this.point(e.offsetX, e.offsetY);

        //top
        if(mPoint.x >= (this.getX() - this.getHandleSize())
            && mPoint.x <= (this.getX() + this.getW() + this.getHandleSize()) 
            && mPoint.y >= (this.getY() - this.getHandleSize())
            && mPoint.y <= (this.getY() + this.getHandleSize())) 
        {
            return true;
        }

        //right
        if(mPoint.y >= (this.getY() - this.getHandleSize())
            && mPoint.y <= (this.getY() + this.getH() + this.getHandleSize()) 
            && mPoint.x >= ((this.getX() + this.getW()) - this.getHandleSize())
            && mPoint.x <= (this.getX() + this.getW() + this.getHandleSize())) 
        {
            return true;
        }

        //bottom
        if(mPoint.x >= (this.getX() - this.getHandleSize())
            && mPoint.x <= (this.getX() + this.getW() + this.getHandleSize()) 
            && mPoint.y >= ((this.getY() + this.getH()) - this.getHandleSize())
            && mPoint.y <= (this.getY() + this.getH() + this.getHandleSize())) 
        {
            return true;
        }

        //left
        if(mPoint.y >= (this.getY() - this.getHandleSize())
            && mPoint.y <= (this.getY() + this.getH() + this.getHandleSize()) 
            && mPoint.x >= (this.getX() - this.getHandleSize())
            && mPoint.x <= (this.getX() + this.getHandleSize())) 
        {
            return true;
        }

        return false;
    }

    IsOnMovingSpot(e) {

        //console.log('acutal place', this.point(this.x, this.y));
        //console.log('this scale', this.scale);
        //console.log('this.han', this.getHandleSize());
        var m = this.point(e.offsetX, e.offsetY);
        //console.log('IsOnMovingSpot id ', this.id);
        //console.log('m', m);
        //console.log('xy point', this.point(this.getX(), this.getY()));
        


        if(this.isOnSpot(this.point(this.getX(), this.getY()), m)) {

            this.movingType = 'topleft';

        } else if(this.isOnSpot(this.point(this.getX() + (this.getW() / 2), this.getY()), m)) {

            this.movingType = 'top';

        } else if(this.isOnSpot(this.point(this.getX() + this.getW(), this.getY()), m)) {

            this.movingType = 'topright';

        } else if(this.isOnSpot(this.point(this.getX() + this.getW(), this.getY() + (this.getH() / 2)), m)) {

            this.movingType = 'right';

        } else if(this.isOnSpot(this.point(this.getX() + this.getW(), this.getY() + this.getH()), m)) {

            this.movingType = 'bottomright';

        } else if(this.isOnSpot(this.point(this.getX() + (this.getW() / 2), this.getY() + this.getH()), m)) {

            this.movingType = 'bottom';

        } else if(this.isOnSpot(this.point(this.getX(), this.getY() + this.getH()), m)) {

            this.movingType = 'bottomleft';

        } else if(this.isOnSpot(this.point(this.getX(), this.getY() + (this.getH() / 2)), m)) {

            this.movingType = 'left';

        } else if(this.IsOnBorder(e)) {

            this.movingType = 'center';
            this.prevPoint = this.point(e.offsetX / this.scale, e.offsetY / this.scale);
        
        } else {

            this.movingType = '';
            this.prevPoint = null;
        }

        //console.log('is movingspo', this.movingType);

        return this.movingType != '';
    }

    GetMouseMovingType(e) {

        var m = this.point(e.offsetX, e.offsetY);

        if(this.isOnSpot(this.point(this.getX(), this.getY()), m)) {

            //this.movingType = 'topleft';
            return 'nwse-resize';

        } else if(this.isOnSpot(this.point(this.getX() + (this.getW() / 2), this.getY()), m)) {

            //this.movingType = 'top';
            return 'ns-resize';

        } else if(this.isOnSpot(this.point(this.getX() + this.getW(), this.getY()), m)) {

            //this.movingType = 'topright';
            return 'nesw-resize';

        } else if(this.isOnSpot(this.point(this.getX() + this.getW(), this.getY() + (this.getH() / 2)), m)) {

            //this.movingType = 'right';
            return 'ew-resize';

        } else if(this.isOnSpot(this.point(this.getX() + this.getW(), this.getY() + this.getH()), m)) {

            //this.movingType = 'bottomright';
            return 'nwse-resize';

        } else if(this.isOnSpot(this.point(this.getX() + (this.getW() / 2), this.getY() + this.getH()), m)) {

            //this.movingType = 'bottom';
            return 'ns-resize';

        } else if(this.isOnSpot(this.point(this.getX(), this.getY() + this.getH()), m)) {

            //this.movingType = 'bottomleft';
            return 'nesw-resize';

        } else if(this.isOnSpot(this.point(this.getX(), this.getY() + (this.getH() / 2)), m)) {

            //this.movingType = 'left';
            return 'ew-resize';

        } else if(this.IsOnBorder(e)) {

            //this.movingType = 'center';
            return 'move';
        
        } else {
            return null;
        }
    }

    Move(e) {

        var mx = e.offsetX / this.scale;
        var my = e.offsetY / this.scale;

        if(this.movingType == 'topleft') {

            this.w = (this.x + this.w) - mx;
            this.x = mx;
            this.h = (this.y + this.h) - my;
            this.y = my;
      
        } else if(this.movingType == 'top') {
            
            this.h = (this.y + this.h) - my;
            this.y = my;
      
        } else if(this.movingType == 'topright') {
            
            this.h = (this.y + this.h) - my;
            this.y = my;
            this.w = mx - this.x;
      
        } else if(this.movingType == 'right') {
            
            this.w = mx - this.x;
      
        } else if(this.movingType == 'bottomright') {
            
            this.w = mx - this.x;
            this.h = my - this.y;
      
        } else if(this.movingType == 'bottom') {
            
            this.h = my - this.y;

        } else if(this.movingType == 'bottomleft') {
            
            this.w = (this.x + this.w) - mx;
            this.x = mx;
            this.h = my - this.y;
      
        } else if(this.movingType == 'left') {
            
            this.w = (this.x + this.w) - mx;
            this.x = mx;
      
        } else if(this.movingType == 'center') {
            
            var m = this.point(mx, my);
            //console.log('new point', m);
            //console.log('prev point', this.prevPoint);
      
            this.x = this.x + (m.x - this.prevPoint.x);
            this.y = this.y + (m.y - this.prevPoint.y);
      
            this.prevPoint = m;
        }
    }

    Select(canvas) {
        //console.log('Select', this);

        var cx = canvas.getContext('2d');
        this.drawCorner(cx, this.GetTopLeftPoint());
        this.drawCorner(cx, this.GetTopPoint());
        this.drawCorner(cx, this.GetTopRightPoint());
        this.drawCorner(cx, this.GetRightPoint());
        this.drawCorner(cx, this.GetBottomRightPoint());
        this.drawCorner(cx, this.GetBottomPoint());
        this.drawCorner(cx, this.GetBottomLeftPoint());
        this.drawCorner(cx, this.GetLeftPoint());

        var lineWidth = this.GetLineWidth();

        //
        cx.beginPath();
        cx.lineWidth = 1;
        cx.strokeStyle = "#3183c7";
        cx.rect(this.getX() - this.getLineToMove(), this.getY() - this.getLineToMove(), this.getW() + lineWidth, this.getH() + lineWidth);
        cx.stroke();
    }

    isBigEnough() {

        //TODO
        return Math.abs(this.w) >= 15 && Math.abs(this.h) >= 15;
    }

    FixBeforeSave() {

        if(this.h < 0) {

            this.h = Math.abs(this.h);
            this.y = this.y - this.h;
        }

        if(this.w < 0) {

            this.w = Math.abs(this.w);
            this.x = this.x - this.w;
        }
    }

    IsChanged(backup) {

        return (this.x != backup.x 
            || this.y != backup.y
            || this.h != backup.h
            || this.w != backup.w
            );
    }

    private GetLineWidth() {
        return 5 * this.scale;
    }
    private getLineToMove() {
        return this.GetLineWidth() / 2;
    }
    private GetTopLeftPoint() { 
        return this.point(this.getX() - this.getLineToMove(), this.getY() - this.getLineToMove()); 
    }
    private GetTopPoint() { 
        return this.point(this.getX() + this.getW() / 2, this.getY() - this.getLineToMove()); 
    }
    private GetTopRightPoint() { 
        return this.point(this.getX() + this.getW() + this.getLineToMove(), this.getY() - this.getLineToMove()); 
    }
    private GetRightPoint() { 
        return this.point(this.getX() + this.getW()  + this.getLineToMove(), this.getY() + this.getH() / 2); 
    }
    private GetBottomRightPoint() { 
        return this.point(this.getX() + this.getW()  + this.getLineToMove(), this.getY() + this.getH()  + this.getLineToMove()); 
    }
    private GetBottomPoint() { 
        return this.point(this.getX() + this.getW() / 2, this.getY() + this.getH() + this.getLineToMove()); 
    }
    private GetBottomLeftPoint() { 
        return this.point(this.getX() - this.getLineToMove(), this.getY() + this.getH() +  + this.getLineToMove()); 
    }
    private GetLeftPoint() { 
        return this.point(this.getX() - this.getLineToMove(), this.getY() + this.getH() / 2); 
    }

    private point(x, y) {
        return {
            x: x,
            y: y
        };
    }

    private isOnSpot(pnt1, pnt2){

        return pnt1.x >= (pnt2.x - this.getHandleSize()) 
              && pnt1.x <= (pnt2.x + this.getHandleSize()) 
              && pnt1.y >= (pnt2.y - this.getHandleSize()) 
              && pnt1.y <= (pnt2.y + this.getHandleSize());
      }

    private drawCorner(cx, point) {

        cx.beginPath();
        cx.fillStyle = "#3183c8";
        //cx.fillStyle = "#3183c7";
        cx.arc(point.x, point.y, 5, 0, 2 * Math.PI);
        //cx.arc(newx, newy, this.getHandleSize(), 0, 2 * Math.PI);
        cx.fill();
    }
}



export enum ShapeType {
    Rectangle,
    line,
    Ellipse,
}

