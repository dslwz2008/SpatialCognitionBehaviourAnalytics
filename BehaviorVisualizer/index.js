/**
 * Item Name :
 * Creater : ShenShen
 * Email : peijiqiu@gmail.com
 * Created Date : 2017/7/6.
 */
// minimal heatmap instance configuration
var heatmapInstance = h337.create({
    // only container is required, the rest will be defaults
    container: document.querySelector('.heatmap')
});

// now generate some random data
var points = [];
var max = 0;
var width = 840;
var height = 400;
var len = 200;


    var point = {
        x: 0,
        y: 0,
        value: 25
    };
    points.push(point);

var point = {
    x: 800,
    y: 0,
    value: 50
};
points.push(point);

var point = {
    x: 0,
    y: 400,
    value: 75
};
points.push(point);

var point = {
    x: 800,
    y: 400,
    value: 100
};
points.push(point);


// heatmap data format
var data = {
    max: 100,
    data: points
};
// if you have a set of datapoints always use setData instead of addData
// for data initialization
heatmapInstance.setData(data);