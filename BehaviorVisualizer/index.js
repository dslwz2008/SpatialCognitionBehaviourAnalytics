/**
 * Item Name :
 * Creater : ShenShen
 * Email : peijiqiu@gmail.com
 * Created Date : 2017/7/6.
 */

var config = {
    container: document.querySelector('.heatmap'),
    radius: 10,
    blur: .75,
    //gradient: {
    //    // enter n keys between 0 and 1 here
    //    // for gradient color customization
    //    '.0': 'blue',
    //    '.5': 'green',
    //    '1': 'red'
    //}
};

var heatmapInstance = h337.create(config);

var width = 526;
var height = 658;
//var categories = ['GazeInPlace','Gaze'];
//var categories = ['Gaze'];
var categories = ['Scan'];
//var categories = ['GazeInPlace','ScanInPlace'];

// heatmap data format
var data = {max:0,data:[]};
for(var indx in categories){
    var category = categories[indx];
    if(all[category].max > data.max)
        data.max = all[category].max;
    data.data = data.data.concat(all[category].data);
}
//data.max = 10;
// if you have a set of datapoints always use setData instead of addData
// for data initialization
heatmapInstance.setData(data);