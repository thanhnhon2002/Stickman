using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
    public static DrawLine instance;
    public LineRenderer lineRenderer;
    [SerializeField] List<Vector2> listPoint = new List<Vector2>();
    Vector2 oldPos;
    Vector2 mousePos;
    PolygonCollider2D polygonCollider2D;
    Rigidbody2D rb;
    public float sizePolygon;
    public float spaceLine;
    public bool isDrawing;
    public bool isDraw;
    [SerializeField] float lengthLine;
    Slider lineBar;
    private void Awake()
    {
        this.lineRenderer = GetComponentInChildren<LineRenderer>();
        this.polygonCollider2D = GetComponentInChildren<PolygonCollider2D>();
        this.rb = GetComponentInChildren<Rigidbody2D>();
        lineBar = FindObjectOfType<Slider>();
        instance = this; 
    }
    private void Start()
    {
        lengthLine = 0;
        isDrawing = false;
        isDraw = true;
        this.rb.simulated = false;
    }
    void Update()
    {
        lineBar.value = lengthLine / GameManager.instance.lengthLines[GameManager.instance.levelIndex];
        this.mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)) this.OnMouseBnDown();
        if (Input.GetMouseButton(0)) this.OnMouseBnDrag();
        if (Input.GetMouseButtonUp(0)) this.OnMouseBnUp();
    }
    public void ResetLine()
    {
        lengthLine = 0;
        lineRenderer.transform.position = Vector3.zero;
        lineRenderer.transform.rotation = Quaternion.identity;
        this.rb.simulated = false;
        this.listPoint.Clear();
        this.lineRenderer.positionCount = 0;
        this.polygonCollider2D.pathCount = 0;
    }
    void OnMouseBnDown()
    {
        if (!isDraw) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        this.isDrawing = true;
        this.ResetLine();
        this.lineRenderer.positionCount = 1;
        this.oldPos = this.mousePos;      
        this.listPoint.Add(this.mousePos);
        this.lineRenderer.SetPosition(0, this.mousePos);
    }
    void OnMouseBnDrag() 
    {
        if(!this.isDrawing) return;
        
        if (Vector3.Distance(this.mousePos, oldPos) < this.spaceLine) return;
        lengthLine += Vector3.Distance(mousePos, oldPos);
        this.oldPos = this.mousePos;      
        this.listPoint.Add(this.mousePos);
        this.lineRenderer.positionCount++;
        this.lineRenderer.SetPosition(this.lineRenderer.positionCount - 1, this.mousePos);

        if (lineBar.value==1) OnMouseBnUp();
    }
    void OnMouseBnUp()
    {
        if (listPoint.Count < 3)
        {
            ResetLine();
            return;
        }
        if (!this.isDrawing) return;
        this.DrawPolygonLine();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        this.rb.simulated = true;
        this.isDrawing = false;
        GameManager.instance.StartLevel();
        isDraw = false;
    } 
    void DrawPolygonLine()
    {
        Vector2[] list = new Vector2[4];
        this.polygonCollider2D.pathCount = listPoint.Count-1;
        for (var i = 1; i < listPoint.Count; i++)
        {
            list[0].x = listPoint[i - 1].x;
            list[0].y = listPoint[i - 1].y+this.sizePolygon;

            list[1].x = listPoint[i-1].x;
            list[1].y = listPoint[i-1].y-this.sizePolygon;

            list[3].x = listPoint[i].x;
            list[3].y = listPoint[i].y + this.sizePolygon;

            list[2].x = listPoint[i].x;
            list[2].y = listPoint[i].y - this.sizePolygon;

            this.polygonCollider2D.SetPath(i-1, list);
        }
    }  
    
}
